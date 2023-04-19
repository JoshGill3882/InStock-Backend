# InStock Backend

Welcome to the repository for the InStock Backend. This is a web API built using ASP .NET Core 6.0. The API is responsible for managing the data for the InStock mobile application, including user information, inventory data, and sales data. The API relies on several key third-party dependencies, including Firebase for their messaging service, AWS DynamoDB for data storage, and AWS S3 for image storage.

While it is not required, we recommend installing Docker for building and running the project. However, this `readme` provides instructions for building and running the project using both Docker and .NET.

## Getting Started

To get started with the InStock backend, please clone this repository:
```
https://git.cardiff.ac.uk/cm6331-2022-23-1/instock-backend.git
```

There are two ways to build and run the application: using Docker or .NET (ensure you have [Set Up User Secrets](#setting-up-user-secrets) before running these commands).

### Building and Running with Docker

If you prefer to use Docker, navigate to the root directory of the project and run the following commands in your terminal to build and run the application:
```
docker build --target final -t 'TAG_NAME' -t 'REPO_NAME' path/to/project
```

Then:

``` 
docker run -d -p 80:80 REPO_NAME:TAG_NAME
```

### Building and Running with .NET

If you prefer to use .NET, navigate to the root directory of the project and run the following commands in your terminal to build and run the application:
```
dotnet build
```

Then:
```
dotnet run
```

_Ensure you have the required dependencies installed. Please note that using .NET requires more manual configuration than using Docker._


## Setting Up User Secrets

In order to keep sensitive information such as database connection strings and API keys secure, it is important to set up app secrets for your application. App secrets are stored in a secure storage location and can be accessed by the application at runtime.

To set up app secrets for the project, please refer to the [official Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows).

Your `secrets.json` should something like this:
```
{
  "jwt:SecretKey": "JWT_SECRET_KEY",
  "AWS_DYNAMO_DB_SECRET_KEY": "DYNAMODB_SECRET_KEY",
  "AWS_DYNAMO_DB_ACCESS_KEY": "DYNAMODB_ACCESS_KEY",
  "AWS_S3_ACCESS_KEY": "S3_ACCESS_KEY",
  "AWS_S3_SECRET_KEY": "S3_SECRET_KEY",
  "SENDER_EMAIL": "SENDER_EMAIL",
  "SENDER_PASSWORD": "SENDER_PASSWORD",
  "FIREBASE_SECRET_KEY": {
    "type": "TYPE",
    "project_id": "PROJECT_ID",
    "private_key_id": "PRIVATE_KEY_ID",
    "private_key": "PRIVATE_KEY",
    "client_email": "CLIENT_EMAIL",
    "client_id": "CLIENT_ID",
    "auth_uri": "https://accounts.google.com/o/oauth2/auth",
    "token_uri": "https://oauth2.googleapis.com/token",
    "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
    "client_x509_cert_url": "CLIENT_X509_CERT_URL"
  }
}
```

## App Deployment

We have created a GitLab Runner to build, test, and deploy the InStock Backend to our AWS EC2 instance. The GitLab Runner is configured to run the build and test stages on a Docker container, and the deploy stage is executed using SSH to connect to the EC2 instance.