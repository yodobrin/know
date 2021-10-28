# Knowledge Mining - leveraging Azure Search
While working directly on the portal seem reasonable, it is appropriate for simple trials, and learning. For other reasons such as product development, it is recommended to use the rest api, for configuration of index, skillset and indexers.
This repo provide few sample such scripts, it uses the vscode REST extention and its parameter/variable scheme.

## Structure
The repo contains two main directories:
- server - contains azure function, that is used as enrichment capability. it accepts text in its payload and return with the top 10 words. note that this folder also contains a `test.rest` file, used to test the function.
- search_scripts - contains the scripts used for skillset, index and indexer


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