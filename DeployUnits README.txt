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