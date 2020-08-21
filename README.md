# 🙌 OpenUserManager 🙌

## API server for authentication, authorization, and user management.
## 🔨 Easy to integrate with anything, anywhere. Also it's free, with all its features, forever.

### ✔️ Features roadmap:
- Asynchronous and secure authentication and authorization.
- JWT token.
- Refresh tokens.
- Claims and roles.
- Admin endpoints for management.
- Third party oauth services(twitter, google).
- SDK for developers.
- Onion architecture, easy to maintain/add new features.
- Swagger documentation.
- Requests validation.
- Db tables to log users metadata.
- Response caching.
- Health checks.
- Versioned API.
- (Future features)
	- AWS IAM authentication
	- Easier cloud deploy using Terraform and environment files.
	- Strong security tests and analysis.
	- MFA (Multi-factor authentication).
	- SSO (Single Sign On).

### 🚀 Launch:

- Debug mode with appsettings.development.json environment variables:
```sh
run-devEnv.bat/sh
```

- Production deploy with appsettings.production.json variables:
```sh
run-releaseEnv.bat/sh
```
Take in mind you should get the production appsettings file not provided in this repository for security.
Also, you should change the admin default user environment variables in Dockerfile.

