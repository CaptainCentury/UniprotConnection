# Summary UniprotConnection

Web service for connecting to UniProt. No error handling whatsoever.

# UniprotService

To run the UniprotService you should have an installation of .Net core on your computer.

VS Code (UniprotConnection/UniprotService):
> dotnet run

https://localhost:5001/api/uniprot/<id>

Ex: https://localhost:5001/api/uniprot/P21802
    https://localhost:5001/api/uniprot/P21820 

(first with comments on function, second without)

# UniprotConnectionTests
Unit tests