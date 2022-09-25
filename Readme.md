![Continuous Integration and Deployment](https://github.com/iamflorent/kata_avid/actions/workflows/ci-cd.yml/badge.svg)

## Brief Technique Aviv ( Groupe SeLoger)


Nous voulons mettre en place un système de dépôt d’annonces pour un grand site bien
connu dans le domaine de l’immobilier.
Une annonce est modélisée de la façon suivante :
- Titre
- Description
- Localisation
- Type de Bien (Maison, Appartement, Parking)

Vous êtes libre de compléter cette modélisation avec les champs qui vous paraissent
pertinents

Le processus de publication d’une annonce est le suivant :
- Création de l’annonce par le propriétaire
- L’annonce est stockée en base de données
- La création de l’annonce retourne l’ID de l’annonce
- Une annonce qui vient d’être créée a le statut “en attente de validation”
- Un Get sur l’ID de l’annonce retourne un 404 tant qu’elle n’a pas le statut
“publiée”.
- Validation de l’annonce par le service client : l’annonce passe du statut “en attente de
validation” à “publiée”
- Publication de l’annonce sur le site :
- Une annonce publiée devient accessible sur son l’url /annonce/id
- Une annonce s’affiche avec la météo en temps réel. Pour cela vous ferez un
appel à l’api https://open-meteo.com/en/docs pour obtenir un ensemble de
données météo de votre choix qui seront affichées sur l’annonce.
Vous n’êtes pas tenu de faire figurer les différents rôles (service client, propriétaire) dans
votre solution.


## Objectifs

Proposer une solution technique en .Net 6 (et plus) permettant la mise en place de ce
processus de publication. Il est attendu une solution backend, le front end est facultatif.

## Instructions

● Votre code devra suivre les bonnes pratiques du Clean Code.
● Votre code devra être testé unitairement (au moins les parties qui vous semblent
importantes)
● La solution devra être mise à disposition sur votre environnement gitHub.
● Optionnel :
	○ Déploiement & delivery
	○ Design for failur
