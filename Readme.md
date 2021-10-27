# Knowledge Mining - leveraging Azure Search


### Setup
You will need to create a .env file at the same location of the rest scripts - this is the content it will require:
```
search_base_url=https://<your search resource name>.search.windows.net/
api_key=<search admin key>
cog_key=<cognitive key>
cog_sub=<cognitive resource fullname>
data_container=<container name of files>
storage_cs=<connection string to storage>
storage_cont=<container name of enriched data>
```

Each of the `rest` script files has few references to the `.env` file but also defines few variables, name and such.