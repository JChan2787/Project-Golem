using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Data;
using Mono.Data.SqliteClient;

/*
* Programmed By: Jay Chan Jr.
* Date         : 15/MAY/2015
*/

/***************************************************************************************
 * CLASS: DeployUnits                                                                  *
 * ------------------------------------------------------------------------------------*
 * The DeployUnits class is called when the player has selected a level. This class    *
 * will be in charge of which unit was selected to be used for the level. Upon         *
 * selection, the player can un-deploy the unit should they change their mind. Once    *
 * four units has been selected, a pop menu window will appear confirming their choice.*
 * If they decide to cancel, the list is emptied and the player has to once again      *
 * choose four units. If the Proceed button is pushed, the game will remember the units*
 * the player has chosen to deploy and proceed to the actual game level. The spawn     *
 * system will also remember these units, and only them to be the units be bought for  *
 * the current level.                                                                  *
 *-------------------------------------------------------------------------------------*
 * MEMBER METHODS:                                                                     *
 *                                                                                     *
 * Public / Non-Helper Methods --                                                      *
 *                                                                                     *
 * NextButtonPushed                                                                    *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * PrevButtonPushed                                                                    *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * AddButtonPushed                                                                     *
 * -> Parameers: None                                                                  *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * RemoveButtonPushed                                                                  *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * CancelButtonPressed                                                                 *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * ProceedButtonPressed                                                                *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 *                                                                                     *
 * Private / Helper Methods --                                                         *
 *                                                                                     *
 * PopulateStateInfo                                                                   *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * CheckIfReady                                                                        *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * RemoveGameObjectUnitIcon                                                            *
 * -> Parameters: int indexRemoveUnit                                                  *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * PopulateIcon                                                                        *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 **************************************************************************************/

public class DeployUnits : MonoBehaviour
{
    //Declaring local members

    //Private -- 
    private GameObject bigIcon;
    private int tempMaxUnitsToDeploy = 4;
    private int listIndex = 0;
    private int deployedCount = 0;
    private List<DeployedUnitInfo> deployed;
    

    //Public --
    public Text Stats;
    public Text UnitName;
    public GameObject temp;
    public List<GameObject> smallIconsOfUnitsDeployed;
    public GameObject popUpReadyWindow;
    public List<Transform> menuDeployedUnitTransform;

    //Method: DeployedUnitInfo
    //Purpose -- A struct that contains all the information 
    //about the unit the player has picked to deploy.
    private struct DeployedUnitInfo
    {
        public string unitName;
        public int unitDmg;
        public int unitHP;
        public float unitAtkRate;
        public float unitMoveSpeed;
        public int unitCost;
    }


    // Use this for initialization
    void Start()
    {
        //Instantiating the list of structs
        deployed = new List<DeployedUnitInfo>();
        
        //This method is called in order to populate menu with 
        //information about the current unit in the list, which
        //in this case, should be the first element in the list.
        PopulateStatInfo();
        
        //Instantiates the icon for the current unit to be displayed. 
        //The name of the prefab should be the same as the one in the 
        //list inside the Player Profile class.
        bigIcon = Instantiate(Resources.Load("TEST Unit Prefabs/" + TestPlayerProfile.instance.GetNameAt(listIndex), 
                              typeof(GameObject))) as GameObject;

        //Set this to false so the pop window won't appear until the 
        //list of selected units has been filled.
        popUpReadyWindow.SetActive(false);
    }

    //Method: PopulateStatInfo
    //Purpose -- This is called to populate the menu about the current
    //unit object that is selected in the list.
    void PopulateStatInfo()
    {
        //Assigns the text field in the UI with the current unit name
        UnitName.text = "Unit Name: " + TestPlayerProfile.instance.GetNameAt(listIndex) + '\n';

        //Assigns the text field with the rest of the unit's stats
        Stats.text = "Health : " + TestPlayerProfile.instance.GetHPAt(listIndex).ToString() + '\n'
                         + "Damage : " + TestPlayerProfile.instance.GetDmgAt(listIndex).ToString();
    }

    //Method: NextButtonPushed()
    //Purpose -- Called when the Next button is pressed by the player. Cycles through the
    //list to get the next element and assign it to be the current unit.    
    public void NextButtonPushed()
    {
        //Checks if the index value is equal to the list size inside the Player Profile
        if(listIndex == TestPlayerProfile.instance.GetInfoListCount() - 1)
        {
            //Assign it back to zero if true
            listIndex = 0;
        }
        else
        {
            //Go the next unit in the list
            listIndex++;
        }

        //This method is called in order to populate menu with 
        //information about the current unit in the list.
        PopulateStatInfo();

        //Since the previous icon is no longer being displayed,
        //destroy it.
        Destroy(bigIcon);

        //Instantiates the icon for the current unit to be displayed. 
        //The name of the prefab should be the same as the one in the 
        //list inside the Player Profile class.
        bigIcon = Instantiate(Resources.Load("TEST Unit Prefabs/" + TestPlayerProfile.instance.GetNameAt(listIndex), 
                              typeof(GameObject))) as GameObject;
    }

    //Method: PreviousButtonPushed()
    //Purpose -- Called when the Prev button is pressed by the player. Cycles through the
    //list to get the previous element and assign it to be the current unit.    
    public void PrevButtonPushed()
    {
        //Checks if the list index is zero, meaning if the index is
        //pointing to the first element
        if(listIndex == 0)
        {
            listIndex = 0;
        }
        else
        {
            //Decrement the value to go to the previous element in the list
            listIndex--;
        }

        //This method is called in order to populate menu with 
        //information about the current unit in the list.
        PopulateStatInfo();

        //Since the previous icon is no longer being displayed,
        //destroy it.
        Destroy(bigIcon);

        //Instantiates the icon for the current unit to be displayed. 
        //The name of the prefab should be the same as the one in the 
        //list inside the Player Profile class.
        bigIcon = Instantiate(Resources.Load("TEST Unit Prefabs/" + TestPlayerProfile.instance.GetNameAt(listIndex), 
                              typeof(GameObject))) as GameObject;
    }

    //Method: AddButtonPushed()
    //Purpose -- The main meat of this class. Called when the player pressed the
    //Add button in the menu. Adds the current unit in the list into the deployed
    //unit list.
    public void AddButtonPushed()
    {
        //Declaring local variables
        DeployedUnitInfo tmp;

        //Checks if the deployed counter is less than max. alloted slots to deploy, and another check
        //if the name of the current unit about to be deployed is already in the deployed unit list.
        //If the name is there, don't add it.
        if(deployedCount < tempMaxUnitsToDeploy && 
           !deployed.Exists(name => name.unitName == TestPlayerProfile.instance.GetNameAt(listIndex))) 
        {
            //Instantiates the small icon into a variable called temp.
            temp = Instantiate(Resources.Load("TEST Unit Prefabs/IconGO/" + TestPlayerProfile.instance.GetNameAt(listIndex), 
                               typeof(GameObject))) as GameObject;

            //Adds the instantiated object into the list of deployed icons.
            smallIconsOfUnitsDeployed.Add(temp);
            
            //This block of code adds all the information associated with the deployed unit into the 
            //struct.
            tmp.unitName = TestPlayerProfile.instance.GetNameAt(listIndex);
            tmp.unitDmg = TestPlayerProfile.instance.GetDmgAt(listIndex);
            tmp.unitHP = TestPlayerProfile.instance.GetHPAt(listIndex);
            tmp.unitAtkRate = TestPlayerProfile.instance.GetFireRateAt(listIndex);
            tmp.unitMoveSpeed = TestPlayerProfile.instance.GetMoveSpeedAt(listIndex);
            tmp.unitCost = TestPlayerProfile.instance.GetUnitCostAt(listIndex);

            //Add the instantiated struct into the deployed list of structs
            deployed.Add(tmp);

            //Increments the deployed counter
            deployedCount++;
        }

        //This method is called in order to populate menu with 
        //information about the current unit in the list.
        PopulateIcon();

        //This method is called to check if the user has already 
        //deployed units equal to the max. alloted of units to deploy.
        CheckIfReady();
    }

    //Method: RemoveButtonPushed()
    //Purpose -- Removes the unit in the deployed unit list that is currently
    //being shown in the big icon. Populates the deployed unit list again
    //if a unit has been removed.
    public void RemoveButtonPushed()
    {
        //Declaring local variables
        GameObject tmp;
        int indexToBeRemoved;

        //Checks if the deployed counter is not empty, and another check
        //if the name of the current unit about to be removed is already in the deployed unit list.
        //If the name is there, don't remove it.
        if(deployedCount > 0 &&
           deployed.Exists(name =>
           name.unitName == TestPlayerProfile.instance.GetNameAt(listIndex)))
        {
            //Set the index of unit to remove
            indexToBeRemoved = smallIconsOfUnitsDeployed.FindIndex(item => item.name.ToString() == bigIcon.name.ToString());

            //Temporary keep track which game object to remove
            tmp = smallIconsOfUnitsDeployed[indexToBeRemoved];

            //Remove the the unit ID from the list
            //idOfUnitsDeployed.RemoveAt(indexToBeRemoved);
            deployed.RemoveAt(indexToBeRemoved);

            //Finall remove the small gameobject icon
            Destroy(tmp);
     
            //Remove icon from the list
            RemoveGameObjectUnitIcon(indexToBeRemoved);

            //With one unit removed, decrement our deployed count
            deployedCount--;

            //Populate the deployed list of icons again
            PopulateIcon();
        }
    }

    //Method: CancelButtonPushed()
    //Purpose -- When this button is pressed, the entire deployed list is
    //cleared. The player has to select up to four units again to deploy.
    public void CancelButtonPressed()
    {
        //Declaring local variables
        GameObject tmp;

        //Take the Confirmation Window out of view.
        popUpReadyWindow.SetActive(false);

        //For-loop that will loop through the whole
        //list of deployed units. Will remove the
        //deployed unit objects in ascending order.
        for(int i = 0; i < 4; i++)
        {
            //Temporarily store the first element in the list to
            //the tmp variable
            tmp = smallIconsOfUnitsDeployed[0];

            //Remove the object the at the first element.
            smallIconsOfUnitsDeployed.RemoveAt(0);
            
            //Destroy the tmp object just to be sure.
            Destroy(tmp);
        }

        //Since the list empty, set deployed count to zero.
        deployedCount = 0;

        //Empty the list of struct objects.
        deployed.Clear();
    }

    //Method: ProceedButtonPressed
    //Purpose -- Meant to be only pushed when the player is absolutely
    //confident thay they are ready to play the level. If pressed,
    //the scene will then transition to the level scene.
    public void ProceedButtonPressed()
    {
        //This loop will iterate through the list of structs. 
        //The iterator will then have the current struct object
        //in the list.
        foreach(DeployedUnitInfo info in deployed)
        {
            //Calls the singleton SpawnManager GameObject and stores the values in the 
            //info struct in its AddDeployInfo method. 
            SpawnManager.instance.AddDeployInfo(info.unitName, info.unitDmg, info.unitHP, info.unitAtkRate,
                                                info.unitMoveSpeed, info.unitCost);
        }

        //Once done, finally transition to the next scene, which is the game level.
        SceneChangetest.instance.ToGame();
    }

    //Method: CheckIfReady
    //Purpose -- Helper function that checks if the deployed
    //list of units has aleady reached its max. capacity.
    //Shows the pop window that asks the player if they are
    //indeed ready to go to the game level.
    private void CheckIfReady()
    {
        //Checks if the deployed list of units has aleady reached
        //its max. capacity. In this case, the player can only
        //initially deploy four units max.
        if(smallIconsOfUnitsDeployed.Count == tempMaxUnitsToDeploy)
        {
            //Calls the SetActive method inherited in the Canvas object
            //assigns it to true. In layman's terms, this will allow
            //the pop-up window to be shown in the scene.
            popUpReadyWindow.SetActive(true);
        }
    }

    //Method: RemoveGameObjectUnitIcon
    //Purpose -- Helper method that takes in an integer parameter  
    //that will serve as the index that will point which element 
    //in list to be removed.
    private void RemoveGameObjectUnitIcon(int indexRemoveUnit)
    {
        //Calls the RemoveAt method in the small icons list
        //and passes in the index.
        smallIconsOfUnitsDeployed.RemoveAt(indexRemoveUnit);
    }

    //Method: PopulateIcon
    //Purpose -- Helper method that populates the menu with the GameObject
    //elements in the small icon list
    private void PopulateIcon()
    {
        //Loops through until the iterator has reached the current amount
        //of units the player has chosen to deploy thus far.
        for(int i = 0; i < deployed.Count; i++)
        {
            //This instruction will allow the deployed objects to be shown in the screen.
            //Each element in the small icons list is related to each Transform element in
            //the menu deployed unit transform list. Meaning, that the transform properties
            //in the latter list will be assigned to to the current small unit icon object
            //in the small icons list.
            smallIconsOfUnitsDeployed[i].transform.position = menuDeployedUnitTransform[i].transform.position;
        }
    }
}