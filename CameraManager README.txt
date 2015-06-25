Programmed By: Jay Chan Jr.

/***************************************************************************************
 * CLASS: CameraManager                                                                *
 * ------------------------------------------------------------------------------------*
 * The CameraManager class will be the one responsible for all camera behaviors during *
 * the game play scenes. Whenever a unit is focused on by the player, the camera will  *
 * be following that unit until a new unit is focused, or that is unit is destroyed.   *
 * A panning function is implemented as well in order for the player to pan across the *
 * game level. In addition, there's also a rudimentary zoom-in/zoom-out fuction also   *
 * available, but will be improved upon later.                                         *
 *-------------------------------------------------------------------------------------*
 * MEMBER METHODS:                                                                     *
 *                                                                                     *
 * Public / Non-Helper Methods --                                                      *
 *                                                                                     *
 * CameraPanning                                                                       *
 * -> Parameters: Vector3 mousePosOrigin                                               *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * CameraZooming                                                                       *
 * -> Parameters: float value                                                          *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * UnitFocus                                                                           *
 * -> Parameers: GameObject unitObj                                                    *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * SpawnIconFocus                                                                      *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * Private / Helper Methods --                                                         *
 *                                                                                     *
 * CameraFollowUnit                                                                    *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * CameraFollowSpawn                                                                   *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *                                                                    *
 **************************************************************************************/