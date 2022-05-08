
# JSON Scenarios

**Test in browser :** <https://happyrouette.itch.io/json-scenarios>

## Shelter scenario

In green : right actions sequence  
In pink : resets of objects state

Sometimes, you can be blocked in the scenario due to an object state that is not appropriate at that moment. You can resolve this by clicking on the object to reset its state.

![A diagram of the right sequence](/_Doc/ShelterWithResets.png)

In green : right actions sequence  
In yellow : warning messages

![A diagram with warning messages](/_Doc/ShelterWithWarnings.png)

## Externalized data

All the objects of a scenario are described in *objets.json* in the Resources folder. Here are their different attributes :  
 - name
 - states
 - x position on scene
 - y position on scene
 - if it can be moved by the user
 - image source (= a spritesheet with a visual for all its states)

 Object's images are located in the Resources folder and are loaded when the scenario is chosen.  

At the start of the app the user can choose a scenario by clicking a button. These buttons are loaded in runtime based on the scenarios present in the Resources folder. Since all the files in that folder are grouped together without hierarchy when the app is built, I couldn't list the different folder names to list the scenarios. That's why I use *ScenariosEnumeration.txt* to list scenarios.

## Externalized interactions

The purpose of *matrice.json* is to enumerate the different interactions between objects. This represent the steps of a scenario. It's a dictionnary with the different objects associations as key. A step contains :
 - the objects resulting state changes
 - a message 
 - a message type (none, good, warning, error, reset, gold)

 To associate objects you can drag one to drop on the other.

## How to add a scenario

To add a new scenario you don't have to write any code. You just have to add files in *Assets/_JSON_Scenario/Resources/Scenarios/\<YourScenarioName\>*. Resources of your scenario should be organized like this (obligatory files in bold):  
 - **\<YourScenarioName\>**
    - **Sprites**
        - *spritesheet_obj_1*
        - *spritesheet_obj_2*
        - **Environement**
        - *spritesheet_obj_3*
    - **matrice.json**
    - **objets.json**

As explained above, don't forget to add \<YourScenarioName\> in *ScenariosEnumeration.txt*.  

In Unity slice your spritesheets, one sprite for each state.

objet.json needs to be structured like this :  

    {
        "objets": {
            "hache": {
                "nom": "hache",
                "etats": ["enBonEtat"],
                "x": 726,
                "y": 509,
                "mobile": true,
                "image_src": "Scenarios/Cabane/Sprites/Axe"
            },
            "terre": {
                "nom": "terre",
                "etats": ["sèche", "humide", "paille", "finie"],
                "x": 405,
                "y": 510,
                "mobile": true,
                "image_src": "Scenarios/Cabane/Sprites/Dirt"
            },
            // [...] dictionary continues
        }
    }

matrice.json needs to be structured like this :  

    {
        "étapes": {
            "seau/plein+terre/sèche": {
                "postconditions": ["terre/humide"],
                "messageType": "good",
                "message": "La terre a été humidifiée"
            },
            "faux/enBonEtat+herbe/quiPousse": {
                "postconditions": ["herbe/coupée"],
                "messageType": "good",
                "message": "L'herbe a été coupée"
            },
            "moule/pleinDeTerre+self": {
                "postconditions": ["moule/vide"],
                "messageType": "reset",
                "message": "La terre a été retirée du moule"
            },
            // [...] dictionary continues
        }
    }

Keys of the dictionary are objects associations. The dragged object reference on the left of the + sign, and the reference of the receiver on the right. The reference of an object is expressed as *object/state*. Note that if the receiver equals *self* this means that the step won't be executed on drag and drop but on click instead.

## Scripts explanation

#### ChooseScenarioPanel

Instantiates and initialize a *ScenarioButton* for each scenario found in *ScenariosEnumeration.txt*.  
Also listen for the home button to reset the scenario.

#### ScenarioButton

Permits to set the text and the action of the button.

#### DataParser

When scenario is chosen, this reads the appropriate *objets.json* and *matrice.json* to create a *Panoply* and a *Recipe* C# objects. 

#### EnvironementManager

Set the background and all the objects at their correct place.  
Initializes interactables.  
Remove all objects on scenario reset.

#### Interactable

Allows an object to be dragged and dropped on another, or not, or clicked. Find the right interactable in range to interact with, and changes the state of the object on wich it's attached to.

#### RecipeManager

Executes interactions between objects. Setting objects to their post-step states.

#### ActionDisplayer

Displays actions done by the user in a scrollview and at the bottom of the screen.  
Toggles the scrollview when todo-list icon is clicked.  
Remove all logs on scenario reset.

#### Extensions

Defines extensions methodes that can potentially be used in multiple scripts.
