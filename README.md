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

GetMethods(): Return a list of those objects that need to be taken into account.

ObjectField(): Fill up the field of the component selection, bottow left.
  
## Example
Here you can see a simple controller, with a list of component easy parameters,
![image](https://user-images.githubusercontent.com/61149758/170071670-c9c7e8c2-3e7b-4764-9057-16fbf91de724.png)

And you can find each variable on those, and other components for use, by adding them to the list
![image](https://user-images.githubusercontent.com/61149758/170071866-bd8bd9b5-8467-457c-9157-fb32fc3346f2.png)
