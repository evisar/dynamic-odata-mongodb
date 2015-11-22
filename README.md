# wesent
Dynamic OData service using MongoDB repository (this is work in progress)
The ultimate idea is to allow you to literally draw and prototype your OData API, in the least amount of time.

This was inspired by DeployD.
It has full compatibility for LinqPad, Excel, Office, etc, since it's using OData.

Current version od OData controller does not support metadata only entities, 
thus I create and cache types on the fly with type builder.

ESENT is not hardwired, there is IRepository<TKey,TEntity> interface, which 
could be plugged-in (not yet written the config attribute) and persist data in File, Mongo, Azure, etc.

It has currently support for authentication with Username Credentials only.


![alt tag](https://github.com/evisar/wesent/blob/master/images/diagram.png)

![alt tag](https://github.com/evisar/wesent/blob/master/images/ef%20modeler.png)

![alt tag](https://github.com/evisar/wesent/blob/master/images/linqpad.png)