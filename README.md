AMAZESPACE-SDK V0.0.4
=================
Welcome to the A MAZE./ SPACE "SDK", this is a Unity project in Unity 2020.3.33f1.

The main purpose of this "SDK" is to allow Artists and Developers involved in the A MAZE./ SPACE to easily generate AssetBundles & Packages of their Exhibits and upload them to the review file storage for the A MAZE./ SPACE.   

Prerequisites:
--------------
* Unity 2020.3.33f1 (LTS)
    * Mac and Windows build Support Installed
* An Artist Account for A MAZE./ SPACE
    * If you are a 2023 Nominee/Honorable Mention your specified Email will be preloaded into the system, when you register (either on the Dashboard or on the A MAZE./ SPACE application) it will automatically associate your Artist Profile with your account. 
* [Edit your Booth text/video content on the Dashboard](https://dashboard.amaze-space.com/login)    

Creating & Submitting a Custom A MAZE./ SPACE Exhibit :
-------------------------------------------------------

Open the Assets/Scenes/AMAZE SDK.unity Scene. 
1. Start at the "**1 ---Amaze SDK Login ---**" object in the Hierarchy.
   * Enter Account Details. 
   * Click the **Login** button in your inspector.
   * Select the **2 ---Choose Option---** object in the Hierarchy and select the **Create An Exhibits** button.
      * The SDK will load a diffrent scene up for you.
   * Click the **Fetch My Exhibits** button in your inspector.
   * Expand the **My Exhibits List** and review you have one or more exhibits available.
   * Select the **Invoke** button next to InitialiseBooth, optionally select an index for the booth you want to create a template prefab for. 
2. Select your newly created prefab that was spawned for you in the Hierarchy and Click the **Open** button in the Inspector. 
   * You can add any objects you would like to your prefab.
   * The Green Arch on the Floor is a preview of roughly the space you have available.
       * While the Green arch is the Minimum possible space, there is some leeway on this.
       * Once the layout in the SPACE has been finalized I can tweak and move around the booths that has more content included and show off more. 
       * Animations and Particle effects are supported by AsseBundles and are good at drawing attention. 
   * Please Note AssetBundles do not support adding C# scripts.
3. We're going to Use AssetBundleMagic a wonderful asset to make managing assetbundle building and versioning simple. 
   * If you are still in the Prefab view, there is a back button at the top of the Hierarchy window. 
   * Simply Select "**3 ---AssetBundlesMagic---**" in the Hierarchy view. Then Click the "Build all AssetBundles" button.
       * I've pre-assigned bundle names associated with your account in Step one, so nothing further is needed here.
       * If your asset bundles are not generating correctly you might not have a specific platform build support installed.
4. Select the "**4 ---Export---**" object in your hierarchy.
   * Click The "Export Booth Prefab As Package" button in your inspector
     * This Generates a .UnityPackage file of your prefab in the _Export folder with recursive dependancies
     * This Step while not strictly necessary is basically to upload all your assets along with your AssetBundle, this allows AMAZE Staff to modify your booth and help it utilize as much floor space as available for your placement in the SPACE. 
     * If you would like to skip this step and only submit an AssetBundle (the compiled out assets) please arrange at exhibits@amaze-space.com for someone to help you iterate your assetbundles yourself.    
5. Select the "**5 ---Upload---**" object in your hierarcy
   * Click The "Zip Up Export Folder" button in your inspector
     * This Zips up everything in the _Export folder.
     * You can also include your Game Trailer and any other Screenshots etc. you would like to submit by simply adding it into the _Export folder. 
     * This .zip file generates in your Assets folder, if you can't see it you might have to right click on your Project window and select refresh.  
   * Click the "Upload Zip" button in your inspector
     * This will upload your Zip file to the review data storage where a Human will review your submission. 
     * After Review your Custom Booth will be made live for your Exhibit and replace the default booth.  

Congratulations you've completed the A MAZE./ SPACE custom exhibit submission process. 

The .zip file that was generated during this process contains your whole exhibit(if you generated your unity package), if you want to backup your efforts you can simply backup the zip file.

To restore your efforts:
   * Clone a fresh copy of this repo
   * Extract your .zip
   * Import the UnityPackage into your fresh project. 

Creating & Submitting a Decor Item for the A MAZE./ SPACE :
-----------------------------------------------------------

Open the Assets/Scenes/AMAZE SDK.unity Scene. 
1. Start at the "**1 ---Amaze SDK Login ---**" object in the Hierarchy.
   * Enter Account Details. 
   * Click the **Login** button in your inspector.
   * Select the **2 ---Choose Option---** object in the Hierarchy and select the **Create Decor** button.
      * The SDK will load a diffrent scene up for you.
   * Select the  **1 ---Amaze SDK Decor---** object in the Heirarchy.
   * Expand the **Initialize Decor** and enter a name for your Decor item and click **Invoke**.
2. Select your newly created prefab that was spawned for you in the Hierarchy and Click the **Open** button in the Inspector. 
   * You can add any objects, models, sprites etc you would like to your prefab.
       * Animations and Particle effects are supported by AsseBundles and are good at drawing attention. 
   * Please Note AssetBundles do not support adding C# scripts.
3. We're going to Use AssetBundleMagic a wonderful asset to make managing assetbundle building and versioning simple. 
   * If you are still in the Prefab view, there is a back button at the top of the Hierarchy window. 
   * Make sure that there is no existing folder called **_Exports**, if there is one make sure there is no files in that directory.
   * Simply Select "**3 ---AssetBundlesMagic---**" in the Hierarchy view. Then Click the "Build all AssetBundles" button.
       * If your asset bundles are not generating correctly you might not have a specific platform build support installed.
4. Select the "**4 ---Zip & Upload---**" object in your hierarchy.
   * Click The **Zip Up Decor Assets** button in your inspector.
     * This Generates a zip file of your asset bundle in the _Export folder with recursive dependancies
   * Once the zip file has been generated you can click the **Upload Decor Zip** button.
      * This process may take a while depending on the size of your asset bundle.
      * This process uploads your zipped up asset bundle to the Amaze server where an Amaze admin user will have the ability to load your 
      asset bundle into the amaze space and potentially have it saved there permanently for the whole world to see.

Congratulations you've completed the A MAZE./ SPACE custom decor submission process. 

Submitting a 2D Decor Sprites for the A MAZE./ SPACE :
------------------------------------------------------

Open the Assets/Scenes/AMAZE SDK.unity Scene. 
1. Start at the "**1 ---Amaze SDK Login ---**" object in the Hierarchy.
   * Enter Account Details. 
   * Click the **Login** button in your inspector.
   * Select the **2 ---Choose Option---** object in the Hierarchy and select the **Create Decor Sprite** button.
      * The SDK will load a diffrent scene up for you.
2. * Select the  **1 ---Amaze SDK Decor Sprite---** object in the Heirarchy.
   * Expand the **Initialize Decor Sprite** and drag a sprite into the input box and click **Invoke**.
3. After initializing your sprite you notice a few things: 
   * You sprite will replace the default sprite already in the Enviroment on the **AmazeSpriteDecor**.
   * The values in the inspector will change depending on the settings of the sprite you have just initialized.
   * Feel free to manaully change these settings as you please if you understand how sprites work in Unity, 
   I would recommend changing the **Decor Sprite Name** to something more meaningful.
   * If you would like to tweak the values and see the live preview you can select your image in the project folder that you have initialized
   and tweak those values in the inspector and click the **Apply** button.
   * Go back to scene view and you will see your sprite has changed but the inspector values on the **1 ---Amaze SDK Decor Sprite---** object 
   have not changed, to fix this will simply hit the **Update Image Settings** button and you should see your tweaked values matching the ones 
   you just set.
4. Once you are happy with your changes you can click the **Upload Decor Sprite** button.
      * This process uploads your decor sprite with your settings to the Amaze server where an Amaze admin user will have the ability to load your 
      decor sprite into the amaze space and potentially have it saved there permanently for the whole world to see.

Congratulations you've completed the A MAZE./ SPACE custom decor sprite submission process. 

Creating & Submitting a Custom A MAZE./ SPACE Exhibit :
-------------------------------------------------------
This feature is currently supported but only used for Events, if you are an event manager and want more information please Speak to Andre on Discord. 

Support
-------
If you encounter any problems, have any suggestions, comments or concerns please feel free to reach out to "Andre|A MAZE./ SPACE" on the A MAZE discord server.

Alternatively contact the exhibits@amaze-space.com email address.

Dev Details
-----------
* This SDK Saves values into the PlayerPrefs for ease of persistence of data.
* The Generated files generate into the Prefabs & _Export folders.
* The Files all the content gets generated from can be found in the "DefaultBooth" Folder
* The Console shows each step completed and the next actions to take. 
* Please Feel Free to look around the code, if there are any changes you'd reccomend I am happy to make them. 
* Tested in a Windows Environment. Verified on Mac, if you encounter any problems on Mac please make sure you are using the right Unity Version.    

---------------------

### 3rd Party Assets Used In The SDK
* [Asset Bundle Magic](https://assetstore.unity.com/packages/tools/network/assetbundlemagic-89770)
* [Gridbox Prototype Materials](https://assetstore.unity.com/packages/2d/textures-materials/gridbox-prototype-materials-129127)
* [Easy Buttons](https://github.com/madsbangh/EasyButtons)
* [Newtonsoft Json.NET](https://www.newtonsoft.com/json)
