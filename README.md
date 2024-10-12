# Satisfactory Discord Bot

[French version below](#bot-discord-satisfactory)

Add this [Discord Bot](https://discord.com/oauth2/authorize?client_id=1290768946156273735) to your server.

> [!NOTE]
> This is not an official Satisfactory tool.

## Installation

### Prerequisites

1. Docker
2. A Discord Bot Token ([discord bot management website](https://discord.com/developers))
3. Postgres Database

### Setup

Create a folder for the Bot config file, it will contains the appsettings.json ([template](/SatisfactoryBot/files/appsettings.json)), and the nlog.config (optional)

Run the bot (x64 image):
```
docker run -v /path/to/previously/created/folder/:/app/files/ ghcr.io/inroma/satisfactorybot:latest
```
<br/><br/>
-
<br/>

# Bot Discord Satisfactory

Ajoute ce [Bot Discord](https://discord.com/oauth2/authorize?client_id=1290768946156273735) à ton serveur.

> [!NOTE]
> Ce bot n'est pas un outil officiel Satisfactory.


## Installation

### Prérequis

1. Docker
2. Le Token de votre Bot Discord ([discord bot management website](https://discord.com/developers))
3. Base de données Postgres

### Configuration

Créer un répertoire contenant le fichier appsettings.json ([template](/SatisfactoryBot/files/appsettings.json)), et nlog.config (optionnel)

Exécuter le bot (image x64):
```
docker run -v /chemin/vers-le/repertoire/cree/:/app/files/ ghcr.io/inroma/satisfactorybot:latest
```