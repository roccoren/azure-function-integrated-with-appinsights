# azure-function-integrated-with-appinsights
It's a sample code to integrate Azure Function with App Insights in Azure China. 

As Azure App Insights requires a change to point to Azure China Endpoint, we need to override the Telemetry Channel for Azure Function manully.

Here, I have create a new file named Startup.cs. Azure Function is built as ASP.NET website, so that we can create a Startup.cs as well to change the behaviors. 

In the Startup.cs, some codes have been appended to override the configuration. 

Check the ```Configure``` for your reference. 

### Reference
[Support for Application Insights in the Azure Government Cloud](https://github.com/Azure/azure-webjobs-sdk/issues/2263)

[Azure China IP addresses and Domains used by Application Insights and Log Analytics](https://docs.azure.cn/zh-cn/azure-monitor/app/ip-addresses)