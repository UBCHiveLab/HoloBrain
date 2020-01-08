There is a lot of data contained in the holobrain project, but for certain cases we might not want to include everything.

For this, I added the ability to read text files as configuration parameters to decided which parts of the Brain GameObject should be enabled and menu "rooms" should be accesible. There are a few examples of these sorts of configurations in Resources/Configs/. As you can see, they are just csv files withc lists of gameobjects. Each configuration should have two files, [configName]Holograms.txt and and [configName]Menu.txt

ApplyConfiguration.md is Monobehaviour attached to the HologramCollection/Brain GameObject and it takes care of reading the AppConfiguration and enabling or disabling the corresponding gameObjects.
