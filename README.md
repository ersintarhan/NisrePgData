### Example Usage

###### 

``` c#
//Example poco object.
public class Person 
{
	public string Name { get; set; }
	public string Surname { get; set; }
	public int Age { get; set; }
}
```



Important field Mapping stuff for postgresql result set.

```c#
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
```



```c#
var db = new NisrePgData.DbRepo(new NpgsqlConnectionStringBuilder("connectionString"));
var result  = await db.Query<Person>("functionName");

//Parametric function call
var p = new PgParam();
p.Add("p_id",1,NpgsqlDbType.Integer);
var result1 = await db.Query<Person>("functionName", p);
```



