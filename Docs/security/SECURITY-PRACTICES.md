# Security & Engineering Practices

**Project:** AILicenseRecertification
**Last updated:** October 2026

---

## 1. Purpose

This document records the security practices applied to this repository: how secrets are kept out of source control, how configuration is managed, how the repository is scanned, and how a past credential exposure was remediated. It describes only what has been implemented. Planned work is listed separately in [Future Improvements](#10-future-improvements).

## 2. Security Principles

- **No secrets in source control.** API keys, connection strings, passwords and tokens never live in code or committed config files.
- **Separate configuration from secrets.** Non-sensitive settings may be versioned; sensitive values may not.
- **Assume history is permanent.** Deleting a secret from the latest commit does not remove it from Git history.
- **Verify, don't assume.** Automated scanning is used to check the repository, and its limits are understood (see section 6).

## 3. Secrets Management

| Rule | Status |
|---|---|
| No API keys in source code | Done |
| No credentials in `appsettings.json` | Done |
| No secrets committed to Git | Done |
| .NET User Secrets for local development | Done |
| Configuration read through `IConfiguration` | Done |
| Azure Key Vault for deployed environments | Planned |

**Local development**

```
.NET User Secrets  ->  IConfiguration  ->  Application services
```

**Deployed environments (planned)**

```
Azure Key Vault  ->  Application configuration  ->  Application
```

Secrets are set locally with:

```bash
dotnet user-secrets set "AzureAI:ApiKey" "<your-key>"
```

User Secrets are stored outside the project directory, so they cannot be committed by accident.

## 4. Configuration Management

Settings are split into configuration and secrets.

| Configuration (can be versioned) | Secret (must be protected) |
|---|---|
| Endpoint | ApiKey |
| DeploymentId | ConnectionString |
| ApiVersion | Password, Token |

`appsettings.json` holds only the non-secret structure; the secret value is left empty and supplied at runtime:

```json
"AzureAI": {
  "Endpoint": "<endpoint>",
  "DeploymentId": "gpt-4o",
  "ApiVersion": "2025-01-01-preview",
  "ApiKey": ""
}
```

**Dependency injection.** `OpenAIAgent` no longer reads credentials directly. It receives `IConfiguration` through its constructor:

```csharp
var apiKey = configuration["AzureAI:ApiKey"];
```

This lets the same code run against different sources without changes:

```
Application
    |
IConfiguration
    |-- User Secrets (local)
    |-- Environment variables
    |-- Azure app configuration
    '-- Key Vault (planned)
```

## 5. Git Security & Hygiene

Generated files and local configuration are excluded through `.gitignore`:

```
bin/
obj/
.vs/
.env
appsettings.Local.json
*.publishsettings
```

Build artifacts bloat the repository and can leak machine-specific details. Local config files often contain credentials. Build artifacts that had already been committed were removed from Git tracking.

## 6. Secret Scanning with Gitleaks

[Gitleaks](https://github.com/gitleaks/gitleaks) is run manually against the full commit history, not just the working tree:

```bash
gitleaks git --verbose
```

**Final verification after remediation:** 4 commits scanned, no leaks found.

**Limitations.** Gitleaks detects patterns that may represent secrets. A clean scan is strong evidence, not proof, that a repository contains no secrets. It can miss unusual formats and cannot know whether a flagged value is real.

Scans are currently run by hand. Automated scanning on commit or in CI is listed under Future Improvements.

## 7. Secret Remediation

An earlier scan found credentials in the repository's Git history, not only in current files. The remediation steps were:

```
Secret discovered
  -> Identify affected files and commits
  -> Remove the secret from current source
  -> Move configuration to secure storage (User Secrets)
  -> Rewrite Git history to remove the secret (git-filter-repo)
  -> Re-scan with Gitleaks
  -> Force-update the remote history
  -> Verify the remote repository
```

`git-filter-repo` was used because deleting a file in a new commit leaves the secret retrievable from earlier commits.

**Credential lifecycle:** Discovery -> Revocation -> Removal -> Verification.

The exposed credentials were temporary hackathon credentials that had already expired and been revoked by Microsoft. The history was rewritten anyway, to remove the exposure and to practise the correct process. For live credentials, revoking or rotating comes first, before any history rewrite.

## 8. Local Development Security

- Secrets are stored with `dotnet user-secrets`, never in files inside the repository.
- Local override files (`.env`, `appsettings.Local.json`) are git-ignored.
- Anyone cloning the repository must supply their own secrets; none are shipped.

## 9. Production Security

Not yet implemented. The intended approach is environment-specific secure configuration, with Azure Key Vault as the secret store, read through the same `IConfiguration` abstraction so application code does not change.

## 10. Future Improvements

- [ ] Secret scanning in CI (and/or a pre-commit hook)
- [ ] Azure Key Vault integration
- [ ] Branch protection rules
- [ ] Dependency vulnerability scanning
- [ ] GitHub secret scanning and push protection enabled on the repository

## 11. Security Verification Checklist

- [x] No API keys hardcoded in source
- [x] No connection strings containing credentials committed
- [x] `.gitignore` configured
- [x] Build artifacts removed from Git
- [x] Local secrets moved to .NET User Secrets
- [x] Historical secrets removed from Git history
- [x] Gitleaks scan performed
- [x] Remote repository history rewritten
- [x] Repository verified after cleanup
- [ ] CI secret scanning
- [ ] Azure Key Vault integration
- [ ] Branch protection
- [ ] Dependency vulnerability scanning