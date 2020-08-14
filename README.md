# [DependencyAttributes] Atreebooster DI
The Hierarchy-based Dependency Injection tool.  
Intuitivly manage dependencies of your MonoBehaviours with simple but powerfull [Attributes]:  
 - [Component] - automatically finds the most likely component and injects the marked field with it.  
 - [GlobalComponent] - finds the singleton or traverses scene GameObjects looking for the component of the marked field type.  
 - [ChildComponent] - finds the component in children (descendants).  
 - [ParentComponent] - finds the component in parents (predecessors).  
 - [SiblingComponent] - finds the component in siblings.  
 - [FamilyComponent] - searches for the component starting from parent down along the hierarchy.  
 - [FamilyComponent(true)] - find the component in own tree (form root down along the hierarchy).  
 - [OwnComponent] - looks for the component only in itself (own GameObject).  
 - [ReferenceComponent] - looks for the component in GameObjects marked [ReferencePoint].  
 - [Child(name/index)] - Finds the GameObject by name in children (descendants) or gets one by index.  
 - [Parent(name/index)] - Finds the GameObject by name in parents (predecessors) or gets one by index.  
 - [Sibling(name/index)] - Finds the GameObject by name in siblings or gets one by index.  
 - [Reference(name)] - Finds closest in the hierarchy GameObject by name.  
 - [Root] - Gets own root GameObject or finds one by name if specified.  
  
Named parameters:  
 - string Of - Name of a GameObject to find and apply the attribute's algorythm to. If there are multiple GameObjects with the same name, it finds the closest one in the hierarchy.  
 - int Offset - Offset in hierarchy from "Of" GameObject.  
 - bool Optional - If false, new component/gameobject is created when it's not found; otherwise just warning is displayed and dependency is not injected. (false by default)  
 - bool SkipItself - If true, own GameObject is skiped; otherwise included in search for the component. (false by default)
  
It works with multiple scenes loaded and DDOL objects.  
It also provides MonoBehaviourSingleton allowing you to access the script from anywhere like this: SoundManager.Instance; or [GlobalComponent] SoundManager soundManager;  

# Usage
Copy MonoBehaviourExtended.cs and MonoBehaviourSingleton.cs to your project from https://github.com/kubpica/AtreeboosterDI/tree/master/Assets/AtreeboosterDI

Derive from MonoBehaviourExtended instead of MonoBehaviour. It provides the hierarchy based dependency injection attributes.  
If you want to use Awake() in your script, hide the method (with the 'new' keyword) and call base.Awake();  
  
If you want your script to be Singleton derive from MonoBehaviourSingleton<T> like this:  
 ```c#
public class SoundManager : MonoBehaviourSingleton<SoundManager> {}  
 ```
Place it anywhere in the scene and then you can access it from any script like this: SoundManager.Instance; or [GlobalComponent] SoundManager soundManager;  

# Known issues and tips
 - If you try to inject a GameObject it should be private or marked with [NonSerialized], otherwise it may conflict with the Unity serializer and the dependency may not be injected.
 - Using the [Component] attribute is convenient but you will probably get better performance if you use the more specific ones. You still can use it, just be carefull in extreme cases.
 - If you want to use the [GlobalComponent] attribute with non-MonoBehaviourSingleton<T> components it's better to put them in one of the root gameobjects; otherwise it may be expensive.

# Licence
MIT but credit or review on Unity Asset Store would be nice ;)

# Unity Asset Store version
https://assetstore.unity.com/packages/tools/integration/dependencyattributes-atreebooster-di-157631
