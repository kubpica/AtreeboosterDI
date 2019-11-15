# [DependencyAttributes] Atreebooster DI
The Hierarchy-based Dependency Injection tool.  
Intuitivly manage dependencies of your MonoBehaviours with simple but powerfull [Attributes]:  
 - [Component] - automatically finds the most likely component and injects the marked field with it.  
 - [GlobalComponent] - finds singleton or traverses scene GameObjects looking for the component of marked field.  
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
 - bool Optional - If false, new component/gameobject is created when it's not found; otherwise just warning is displayed and dependency is not injected.  
 - bool SkipItself - If true, own GameObject is skiped; otherwise included in search for the component.  
  
It works with multiple scenes loaded and DDOL objects.  
It also provides MonoBehaviourSingleton allowing you to access the script from anywhere like this: SoundManager.Instance; or [GlobalComponent] SoundManager soundManager;  
  
# Usage
Derive from MonoBehaviourExtended instead of MonoBehaviour. It provides the hierarchy based dependency injection attributes.  
If you want to use Awake() in your script, hide the method (with new keyword) and call base.Awake();  
  
If you want your script to be Singleton derive from MonoBehaviourSingleton<T> like this:  
public class SoundManager : MonoBehaviourSingleton<SoundManager> {}  
Place it anywhere in the scene and then you can access it from any script like this: SoundManager.Instance; or [GlobalComponent] SoundManager soundManager;  
