# Satisfactory Discord Bot
![Command preview](/../docs/docs/command-preview.png?raw=true "Command preview")\
[French version below](#bot-discord-satisfactory)

Add this [Discord Bot](https://discord.com/oauth2/authorize?client_id=1290768946156273735) to your server.\
You can use the bot in DM or in a Discord Server.\
The "/claim" command allows you to add your Server, either with the Token with in-game with command `server.GenerateAPIToken`, or with a password if it is not configured yet.

Some commands are restricted to Discord Admin Roles only (future update will add permission management).

> [!NOTE]
> This is not an official Satisfactory tool.\
> Thanks to CoffeeStain for providing an API.

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
<br/>

> [!NOTE]
> We are open to Pull Request if you would like to add Mods features

<br/><br/>

# Bot Discord Satisfactory

Ajoute ce [Bot Discord](https://discord.com/oauth2/authorize?client_id=1290768946156273735) à ton serveur.\
Ce bot est utilisable en MP ou dans un Serveur Discord.\
Utilise la commande "/claim" pour ajouter un serveur, soit avec le Token obtenu avec la commande `server.GenerateAPIToken`, soit avec un nouveau mot de passe si le server n'est pas encore configuré.

Certaines commandes sont limitées aux rôles Discord avec les droits Administrateur (une update ajoutera la gestion des permissions Discord).

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

> [!NOTE]
> Nous sommes ouverts aux Pull-Request afin d'ajouter des fonctionnalités de Mods.
