`<configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
  <!--
    In the example below, the "SetAttributes" transform will change the value of
    "connectionString" to use "ReleaseSQLServer" only when the "Match" locator
    finds an atrribute "name" that has a value of "MyDB".

    <connectionStrings>
      <add name="MyDB"
        connectionString="Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True"
        xdt:Transform="SetAttributes" xdt:Locator="Match(name)"/>
    </connectionStrings>
  -->
  <appSettings>
    <add key="ida:Tenant" value="[YOURTENANT].onmicrosoft.com" />
    <add key="ida:ClientId" value="[YOURCLIENTID]" />
    <add key="ida:AadInstance" value="https://login.microsoftonline.com/{0}/v2.0/.well-known/openid-configuration?p={1}" />
    <add key="ida:RedirectUri" value="https://localhost:44316/" />
    <add key="ida:SignUpPolicyId" value="[SIGNUPPOLICY]" />
    <add key="ida:SignInPolicyId" value="[SIGNINPOLICY]" />
    <add key="ida:UserProfilePolicyId" value="[USERPROFILEPOLICY]" />
    <!-- Azure Storage: afdevpingisne -->
    <add key="AzureStorageConnectionString" value="[YOURAZURESTORAGEACCOUNTCONNECTIONSTRING]" />
  </appSettings>
  <system.web>
    <!--
      In the example below, the "Replace" transform will replace the entire
      <customErrors> section of your Web.config file.
      Note that because there is only one customErrors section under the
      <system.web> node, there is no need to use the "xdt:Locator" attribute.

      <customErrors defaultRedirect="GenericError.htm"
        mode="RemoteOnly" xdt:Transform="Replace">
        <error statusCode="500" redirect="InternalError.htm"/>
      </customErrors>
    -->
  </system.web>
</configuration>`