<div align="center">

  <!-- Website -->
  <a href="https://ensapra.com">
    <img src="https://img.shields.io/badge/Website-Visit-1999?style=for-the-badge&logo=googlechrome&logoColor=white" alt="Ensapra Website" />
  </a>

  <!-- Discord -->
  <a href="https://ensapra.com/links/discord">
    <img src="https://img.shields.io/badge/Discord-Join_Server-5865F2?style=for-the-badge&logo=discord&logoColor=white" alt="Join Ensapra Discord" />
  </a>

  <!-- Patreon -->
  <a href="https://ensapra.com/links/patreon">
    <img src="https://img.shields.io/badge/Patreon-Support-FF424D?style=for-the-badge&logo=patreon&logoColor=white" alt="Support Ensapra on Patreon" />
  </a>

  <a href="https://ensapra.com/links/easy-parameters-asset-store">
    <img src="https://img.shields.io/badge/Asset_Store-Get_for_free!-dba248?style=for-the-badge&logo=unity&logoColor=white" alt="Support Ensapra on Patreon" />
  </a>
  
</div>


# Easy Parameters
This package allows for the modification and connection of Animator Parameters from the editor itself, without hard-writing anything inside scripts.
It allows for extension, by inheriting from EasyParameter, to create a custom method to obtain the diferent objects and variables.

## Scripts
The base package contains the basic scripts to get variables from components by default. No need to create extra scripts, just add ParameterController.cs, and set the animator field.

However, you can implement your own version by creating a script inheriting from EasyParameter, and from EasyParameterDrawer.

## Custom implementation
To do a custom implementation you must create two basic scripts.

1 - Deriving from EasyParameters of the desired Type.

2 - Deriving from EasyParametersDrawer, implementing the two methods. 

GetMethods(): Return a list of those objects that need to be taken into account when selecting a field. The parameter, "component", refers to the object selected. Ex: On the implementation EPComponent, this method returns all the components that can be found in relation to the component selected. 

ObjectField(): Fill up the field of the component selection, bottow left. This field should assign the serializedProperty, "parentObject" to the object containing the desired field to connect with the Animator. Ex: On the implementation EPComponent, this method creates an Object Field from where a component can be selected. 
  
## Example
Here you can see a simple controller, with a list of component easy parameters,
![image](https://user-images.githubusercontent.com/61149758/170071670-c9c7e8c2-3e7b-4764-9057-16fbf91de724.png)

And you can find each variable on those, and other components for use, by adding them to the list
![image](https://user-images.githubusercontent.com/61149758/170071866-bd8bd9b5-8467-457c-9157-fb32fc3346f2.png)
