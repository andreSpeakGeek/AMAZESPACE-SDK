AMAZESPACE-SDK V0.0.1
=================
Welcome to the A MAZE./ SPACE "SDK", this is a Unity project in Unity 2020.3.33f1.

The main purpose of this "SDK" is to allow Artists and Developers involved in the A MAZE./ SPACE to easily generate AssetBundles & Packages of their Exhibits and upload them to the review file storage for the A MAZE./ SPACE.   

Prerequisites:
--------------
* Unity 2020.3.33f1 (LTS)
    * Mac and Windows build Support Installed
* An Artist Account for A MAZE./ SPACE
    * If you are a 2022 Nominee/Honorable Mention your specified Email will be preloaded into the system, when you register (either on the Dashboard or on the A MAZE./ SPACE application) it will automatically associate your Artist Profile with your account. 
* [Edit your Booth text/video content on the Dashboard](https://dashboard.amaze-space.com/login)    

Creating & Submitting a Custom A MAZE./ SPACE Exhibit :
-------------------------------------------------------

Open the Assets/Scenes/AssetBundleCreation.unity Scene. 
1. Start at the "**1 ---Amaze SDK Login ---**" object in the Hierarchy.
   * Enter Account Details. 
   * Click the **Login** button in your inspector.
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
Coming Soon.

Submitting a 2D Decor Sprites for the A MAZE./ SPACE :
------------------------------------------------------
Coming Soon.

Support
-------
If you encounter any problems, have any suggestions, comments or concerns please feel free to reach out to "Andre|A MAZE./ SPACE" on the A MAZE discord server.

Alternatively contact the exhibits@amaze-space.com email address.

Dev Details
-----------
* This SDK Saves values into the PlayerPrefs for ease of persistence of data.
    * Managing an Active Manifest File for the current state of the "SDK" felt like too much complexity just yet.   
* The Generated files generate into the Prefabs & _Export folders.
* The Files all the content gets generated from can be found in the "DefaultBooth" Folder
* The Console shows each step completed and the next actions to take. 
* Please Feel Free to look around the code, however not taking pull requests, this is a snapshot of a Plastic SCM Repo
* Tested in a Windows Environment. Ran once on a MAC to verify, but not battle tested on MAC.  

---------------------

### 3rd Party Assets Used In The SDK
* [Asset Bundle Magic](https://assetstore.unity.com/packages/tools/network/assetbundlemagic-89770)
* [Gridbox Prototype Materials](https://assetstore.unity.com/packages/2d/textures-materials/gridbox-prototype-materials-129127)
* [Easy Buttons](https://github.com/madsbangh/EasyButtons)
* [Newtonsoft Json.NET](https://www.newtonsoft.com/json)