1. How long did you spend on the coding assignment? What would you add to your solution if you had
more time? If you didn't spend much time on the coding assignment then use this as an opportunity to
explain what you would add.

    A. I spent about 20 hours on the assignment. If I had more time I would have also added a version of the api using serverless and event-driven. I would have added the CI/CD pipelines as well. As an extra, I would have added a timeseries for each cryptocurrency as a detailed view in the UI.

2. What was the most useful feature that was added to the latest version of your language of choice?
Please include a snippet of code that shows how you've used it.

    A. While it has not been added in the latest version of C# (10.0), but in the one before (9.0), I still find the addition of records very useful. A sample from the coding assignment:
    ``` C#
    public sealed record CryptoCoin(int Id, string Symbol, string? Name = default) : Coin(Id, Symbol, Name);
    ```

3. How would you track down a performance issue in production? Have you ever had to do this?

    A. If the solution has monitoring, logging and/or dependency tracing in place, I would start with investigating those for any errors, long running dependencies, increased load, etc. I would also check if any change in behaviour correlates with a recent deployment/change. If possible, I would look to reproduce it on other environments.
    
    Yes, I had to do it a few times.

4. What was the latest technical book you have read or tech conference you have been to? What did you
learn?

    A. The latest tech book I have read is Azure Databricks Cookbook. It enabled me to get a better understanding of Databricks as a platform and how to use it more efficiently for data processing.

5. What do you think about this technical assessment?

    A. It was quite straightforward. Nothing too complex, just a matter of finding the time to do it.

6. Please, describe yourself using JSON.

    A. Here you go:
    ```JSON
    {
        "type": "person",
        "kind": "male",
        "version": "1986-11-16T15:25:00.000Z",
        "name": "Eugen Podaru",
        "location": "West Europe",
        "identity": 1,
        "properties": {
            "reserved": true,
            "cors": {
                "allowedOrigins": "*"
            },
            "healthCheck": "enabled",
            "limits": {
                "maxRuntimeInHours": 24,
                "maxMemoryInYears": 31
            },
            "preWarmedInstanceCount": 1,
            "publishingUsername": "devlight",
            "remoteDebuggingEnabled": false,
            "iconUrl": "https://media-exp1.licdn.com/dms/image/C4D03AQFf2AslD23SpQ/profile-displayphoto-shrink_800_800/0/1517345554213?e=1668038400&v=beta&t=T_KWTcJSKNKYBthQztiUemk0gXOkaaUkdW5so-R4fSk"
        }
    }
    ```