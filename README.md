# shopbridge
Shop bridge application of ThinkBridge assement

# available api's

/api/shopbridge/products - HttpGet - To Get All

/api/shopbridge/products - HttPost - To Create

/api/shopbridge/products/{productId} - HttpPut - To Update

/api/shopbridge/products/{productId} - HttpDelete - ToDelete

# validations

1. Used FluentValidations for domain model validations
2. Used the ModelState validations for model errors

# datastore

Used the Local Sql file (.mdf)

# unit tests

For unit tests, used the xUnit framework and for assertions in the unit tests used the fluent assertions.
