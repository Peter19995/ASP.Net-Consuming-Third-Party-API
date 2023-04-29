# Consuming Third-Party API in ASP.NET Web API (Cooperative Bank of Kenya Developer API)
This is an example project demonstrating how to consume a third-party API in an ASP.NET Web API project. In this example, we will be using the Co-op Bank Kenya API as our third-party API.

## Getting Started
### Prerequisites
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
- API credentials from [Co-op Bank Kenya API.](https://developer.co-opbank.co.ke/devportal/apis).
- [MS SQL](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
### Installation
1. Clone the repository.

        git clone https://github.com/Peter19995/Consuming-Third-Party-API.git

2.  Run the Script below to your database.

        CREATE DATABASE  ThirdPartyDB
        go
        DROP TABLE APICredentials
        GO
        CREATE TABLE APICredentials(
            ApiCredentialsId INT IDENTITY(1,1),
            ApiUrl VARCHAR(100),
            ConsumerKey VARCHAR(max),
            SecretKey VARCHAR(max),
            APIkey VARCHAR(max),
            TokenURl VARCHAR(max),
            IsDefault bit,
            Username VARCHAR(MAX),
            DatabaseLogTime DATETIME,
            IsDeleted BIT DEFAULT(0),
            DeletedBy VARCHAR(100) NULL,
            DateDeleted DATETIME NULL,
        )
        GO
        -- Create the CredentialsOperations stored procedure, which handles Create, Soft Delete, and Update operations.
        DROP PROCEDURE CredentialsOperations
        GO
        CREATE PROCEDURE CredentialsOperations
        (
            @ApiCredentialsId INT = 0,
            @ApiUrl VARCHAR(255) = NULL,
            @ConsumerKey VARCHAR(255) = NULL,
            @SecretKey VARCHAR(255) = NULL,
            @APIkey VARCHAR(max) = NULL,
            @TokenURl VARCHAR(max)  = NULL,
            @IsDefault BIT = true,
            @IsValid BIT = true,
            @IsUpdate BIT = false,
            @UserName VARCHAR(255) = NULL,
            @ResponseOut VARCHAR(255) OUTPUT
        )
        AS
        BEGIN
            SET NOCOUNT ON;
            BEGIN TRY
                BEGIN TRANSACTION;
                --Check if is update
                IF (@IsUpdate = 1)
                BEGIN
                    --check if is soft delete
                    IF (ISNULL(@IsValid, 0) = 0)
                    BEGIN
                        UPDATE APICredentials
                        SET IsDeleted = 1,
                            DeletedBy = @UserName,
                            DateDeleted = GETDATE()
                        WHERE ApiCredentialsId = @ApiCredentialsId;

                        SET @ResponseOut = 'Credentials Deleted';
                    END
                    ELSE -- update is is not delete
                    BEGIN
                        UPDATE APICredentials
                        SET ApiUrl = @ApiUrl,
                            ConsumerKey = @ConsumerKey,
                            SecretKey = @SecretKey,
                            APIkey = @APIkey,
                            TokenURl = @TokenURl,
                            IsDeleted = 0,
                            DeletedBy = NULL,
                            DateDeleted = NULL,
                            IsDefault = @IsDefault
                        WHERE ApiCredentialsId = @ApiCredentialsId;

                        SET @ResponseOut = 'Updated Credentials Successfully';
                    END

                    IF(ISNULL(@IsDefault, 0) = 1)
                    BEGIN
                        UPDATE APICredentials
                        SET IsDefault = 0
                        WHERE ApiCredentialsId != @ApiCredentialsId;
                    END
                END
                ELSE -- Create new if is not delete
                BEGIN
                    -- there can only exist one default credentials that API will use to Call Third party API
                    IF NOT EXISTS(SELECT 1 FROM APICredentials WHERE IsDefault = 1)
                    BEGIN
                        INSERT INTO APICredentials(ApiUrl, ConsumerKey, SecretKey,APIkey,TokenURl, IsDefault, Username, DatabaseLogTime) 
                        VALUES (@ApiUrl, @ConsumerKey, @SecretKey,@APIkey,@TokenURl, @IsDefault, @UserName, GETDATE());

                        SET @ResponseOut = 'Created Credentials Successfully';
                    END
                    ELSE
                    BEGIN
                        SET @ResponseOut = 'Default Credentials already exists';
                    END
                END

                COMMIT TRANSACTION;
            END TRY
            BEGIN CATCH
                ROLLBACK TRANSACTION;
                SET @ResponseOut = 'Error: ' + ERROR_MESSAGE();
            END CATCH
        END
        go
        DROP PROCEDURE GetCredentials
        go
        CREATE PROCEDURE GetCredentials 
        ( 
            @Code int, 
            @SearchId int
        ) 
        AS
        BEGIN
            if(@Code = 0)
            BEGIN
                select * FROM APICredentials WITH(NOlock) where isnull(IsDeleted,0) = 0
            END
            if(@Code = 1)
            BEGIN
                select * FROM APICredentials WITH(NOlock) where isnull(IsDeleted,0) = 0 AND ApiCredentialsId = @SearchId
            END
            if(@Code = 2)
            BEGIN
                select * FROM  APICredentials WITH(NOlock) where isnull(IsDeleted,0) = 0 AND isnull(IsDefault,0) = 1
            END
        END
3. In the Web.conifg change the aname of your server.

        <connectionStrings>
            <add name="DefaultConnection" connectionString="Data Source=YourServerName;Initial Catalog=ThirdPartyDB;Integrated Security=True" providerName="System.Data.SqlClient" />
        </connectionStrings>

## Usage
### Adding Co~op Bank API credentials
The API exposes an endpoint /api/credentials that accepts POST requests to add new API credentials. To add new credentials, use the following format:

        {
            "ApiUrl": "https://developer.co-opbank.co.ke:8243/EnrollmentsAPI/1.0.0",
            "ConsumerKey": "YOUR_CONSUMER_KEY",
            "SecretKey": "YOUR_SECRET_KEY",
            "APIkey": "YOUR_API_KEY",
            "TokenURl": "https://developer.co-opbank.co.ke:8243/token",
            "IsDefault": true
        }


To update Credentials use the end point *'/update/credentials'* passing new credential data with *ApiCredentialsId* to be updated.

To get all credential use the endpoint *'/get/credentials'*.
- Pass code = 0  to get all
- Pass code = 1 and SearchId = ApiCredentialsId to pull specific credentials
- Pass code = 2 to get default credentials that will be used by your API when calling Co~op Bank API.

### Calling Co~op Bank API.
Use endpoint *'/get/account/balance'* to call Coop Bank API for account balance.

## Contributing.
Contributions are welcome! If you have any suggestions or bug reports, please open an issue or submit a pull request. 



## License

This project is licensed under the MIT License - see the [ LICENSE](https://opensource.org/licenses/MIT) file for details.

## Acknowledgments
- [Co-opBank Kenya API](https://developer.co-opbank.co.ke/devportal/apis) for providing the API used in this project.

## Conclusion
This project demonstrates how to consume a third party API in an ASP.NET Web API project. By following the steps outlined in this project, you should be able to easily consume any third party API in your own ASP.NET Web API projects.

Plus, I have only called one end point of coopbank API for checking account balance, fill free to add  and consume the rest of endpoint like Account FullStatement, IFT Account To Account, e.t.c 