using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Derive from this instead of <c>MonoBehaviour</c>. If you want to use Awake() in your script, hide the method (with the <c>new</c> keyword) and call <c>base.Awake();</c>
/// It provides the hierarchy based dependency injection attributes.
/// </summary>
/// <example>
/// <c>public class YourMonoBehaviour : MonoBehaviourExtended {}</c>
/// </example>
public class MonoBehaviourExtended : MonoBehaviour
{
    /// <summary>
    /// Notification frequency:
    /// 2 = Double warnings with normal log.
    /// 1 = Display all logs.
    /// 0 = Display only important ones. (Default)
    /// -1 = Display only warnings.
    /// -2 = Not even warnings.
    /// </summary>
    public static int Verbosity { get; set; }

    /// <summary>
    /// Debug.Log() depending on <see cref="Verbosity"/> and <c>quietly</c> parameter.
    /// </summary>
    /// <param name="msg"> Message to (try to) display.</param>
    /// <param name="quietly"> If true the min. <see cref="Verbosity"/> have to be 1 to display the message; otherwise 0.</param>
    protected static void Log(string msg, bool quietly)
    {
        Log(msg, quietly ? 1 : 0);
    }

    /// <summary>
    /// Debug.Log()s if <see cref="Verbosity"/> >= <c>minVerbosity</c>.
    /// </summary>
    /// <param name="msg"> Message to (try to) display.</param>
    /// <param name="minVerbosity"> Verbosity required for the message to be displayed.</param>
    protected static void Log(string msg, int minVerbosity = 0)
    {
        if (Verbosity >= minVerbosity)
            Debug.Log(msg);
    }

    /// <summary>
    /// Debug.LogWarning() depending on <see cref="Verbosity"/> and <c>quietly</c> parameter.
    /// </summary>
    /// <param name="msg"> Warning to (try to) display.</param>
    /// <param name="quietly"> If true the min. <see cref="Verbosity"/> have to be 1 to display the message; otherwise 0.</param>
    protected static void LogWarning(string msg, bool quietly)
    {
        LogWarning(msg, quietly ? 1 : -1);
    }

    /// <summary>
    /// Debug.LogWarning()s if <see cref="Verbosity"/> >= <c>minVerbosity</c>.
    /// </summary>
    /// <param name="msg"> Warning to (try to) display.</param>
    /// <param name="minVerbosity"> Verbosity required for the warning to be displayed.</param>
    protected static void LogWarning(string msg, int minVerbosity = -1)
    {
        if (Verbosity >= 2)
            Debug.Log("Warning: " + msg);
        if (Verbosity >= minVerbosity)
            Debug.LogWarning(msg);
    }

    /// <summary>
    /// Gets root GameObjects of DontDestroyOnLoad scene.
    /// </summary>
    /// <returns> List of DDOL root GameObjects.</returns>
    public static List<GameObject> GetDontDestroyOnLoadObjects()
    {
        var sceneRoots = new List<GameObject>(); // Root GameObjects except DontDestroyOnLoad
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            sceneRoots.AddRange(SceneManager.GetSceneAt(i).GetRootGameObjects());
        }

        var result = new List<GameObject>();
        var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (var t in allTransforms)
        {
            if (t.parent == null) // Is root?
            {
                if (t.hideFlags == HideFlags.None && !sceneRoots.Contains(t.gameObject))
                {
                    result.Add(t.gameObject);
                }
            }
        }

        result.Sort((x, y) => string.Compare(x.name, y.name));

        return result;
    }

    /// <summary>
    /// Do not use it directly, it's just base class for all of dependency attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    protected class DependencyAttribute : PropertyAttribute
    {
        /// <summary>
        /// Name of a GameObject to find and apply the attribute's algorythm to.
        /// </summary>
        /// <remarks>
        /// Searching of the GameObject starts from [ReferencePoint]s then <c>this.gameObject</c>, its children, siblings, children of siblings, then goes to a parent and repeats the algorythm.
        /// If null, <c>this.gameobject</c> is used.
        /// </remarks>
        /// <seealso cref="Offset"/>
        /// <seealso cref="ReferencePointAttribute"/>
        public string Of { get; set; }

        /// <summary>
        /// Offset in hierarchy from <see cref="Of"/> GameObject or <c>this.gameObject</c>.
        /// Positive values go through parents, negative through first children.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// If false, new component/gameobject is created when it's not found; otherwise just warning is displayed and dependency is not injected.
        /// </summary>
        public bool Optional { get; set; }

        public DependencyAttribute() { }

        /// <summary>
        /// <see cref="DependencyAttribute"/>
        /// </summary>
        /// <param name="generationOffset"> <see cref="Offset"/></param>
        public DependencyAttribute(int generationOffset)
        {
            Offset = generationOffset;
        }

        public override string ToString()
        {
            string s = base.ToString();
            return s.Remove(0, s.IndexOf('+')+1);
        }
    }

    /// <summary>
    /// Do not use it directly, it's just base class for all of component dependency attributes.
    /// </summary>
    protected class ComponentDependencyAttribute : DependencyAttribute { public ComponentDependencyAttribute(int generationOffset = 0) : base(generationOffset) { } }
    /// <summary>
    /// Looks for the component only in self - <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/>.
    /// </summary>
    protected class OwnComponentAttribute : ComponentDependencyAttribute { public OwnComponentAttribute(int generationOffset = 0) : base(generationOffset) { } }
    /// <summary>
    /// Automaticly finds the component (traversing and climbing the hierarchy). 
    /// Search order: <see cref="MonoBehaviourSingleton{T}">singletons</see>, [ReferencePoint]s, itself, children, siblings, children of siblings, go to parent and repeat the algorythm.
    /// </summary>
    /// <seealso cref="ReferencePointAttribute"/>
    protected class ComponentAttribute : ComponentDependencyAttribute { public ComponentAttribute(int generationOffset = 0) : base(generationOffset) { } }
    /// <summary>
    /// Finds the component independently from <c>this.gameObject</c>.
    /// Search order: <see cref="MonoBehaviourSingleton{T}">singletons</see>, root GameObjects, children of roots traversed in alphabetical order.
    /// </summary>
    /// <remarks>
    /// Roots are searched in alphabetical order to maintain consistency because order of roots in scene is not guaranteed (editor vs build).
    /// </remarks>
    protected class GlobalComponentAttribute : ComponentDependencyAttribute { public GlobalComponentAttribute() : base(0) { } }
    /// <summary>
    /// Searches for the component at <see cref="ReferencePointAttribute">[ReferencePoint]s</see>.
    /// </summary>
    /// <seealso cref="ReferencePointAttribute"/>
    protected class ReferenceComponentAttribute : ComponentDependencyAttribute {
        /// <summary>
        /// If true, it also searches in children of <see cref="ReferencePointAttribute">[ReferencePoints]s</see>; otherwise only directly in them.
        /// </summary>
        public bool DeepSearch { get; set; }

        /// <summary>
        /// <see cref="ReferenceComponentAttribute"/>
        /// </summary>
        /// <param name="deepSearch"> <see cref="DeepSearch"/></param>
        public ReferenceComponentAttribute(bool deepSearch = false) : base(0)
        {
            DeepSearch = deepSearch;
        }
    }
    /// <summary>
    /// Finds the component in children of <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/>.
    /// </summary>
    protected class ChildComponentAttribute : ComponentDependencyAttribute
    {
        /// <summary>
        /// If true, it searches only in children (descendants) skiping itself (the parent), otherwise it looks also into <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/> for the component.
        /// </summary>
        public bool SkipItself { get; set; }

        /// <summary>
        /// <see cref="ChildComponentAttribute"/>
        /// </summary>
        /// <param name="generationOffset"> <see cref="DependencyAttribute.Offset"/></param>
        public ChildComponentAttribute(int generationOffset = 0) : base(generationOffset) { }
    }
    /// <summary>
    /// Finds the component in parents of <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/>.
    /// </summary>
    protected class ParentComponentAttribute : ComponentDependencyAttribute
    {
        /// <summary>
        /// If true, it searches only in parents (predecessors) skiping itself; otherwise it looks also into <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/> for the component.
        /// </summary>
        public bool SkipItself { get; set; }

        /// <summary>
        /// <see cref="ParentComponentAttribute"/>
        /// </summary>
        /// <param name="generationOffset"> <see cref="DependencyAttribute.Offset"/></param>
        public ParentComponentAttribute(int generationOffset = 0) : base(generationOffset) { }
    }
    /// <summary>
    /// Finds the component in siblings of <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/>.
    /// </summary>
    protected class SiblingComponentAttribute : ComponentDependencyAttribute
    {
        /// <summary>
        /// If true, it searches only in siblings skiping itself; otherwise it looks also into <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/> for the component.
        /// </summary>
        public bool SkipItself { get; set; }
        /// <summary>
        /// <see cref="SiblingComponentAttribute"/>
        /// </summary>
        /// <param name="generationOffset"> <see cref="DependencyAttribute.Offset"/></param>
        public SiblingComponentAttribute(int generationOffset = 0) : base(generationOffset) { }
    }
    /// <summary>
    /// Searches for the component starting from parent (or <see cref="Generations"/> or <see cref="FromRoot"/>) down along the hierarchy.
    /// </summary>
    protected class FamilyComponentAttribute : ComponentDependencyAttribute
    {
        /// <summary>
        /// If true, <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/> is skiped; otherwise included in search for the component.
        /// </summary>
        public bool SkipItself { get; set; }
        /// <summary>
        /// If true, it starts searching from root GameObject of <c>this.gameObject</c> or <see cref="Of"/>.
        /// </summary>
        public bool FromRoot { get; set; }
        /// <summary>
        /// If <see cref="FromRoot"/>, it indicates how many GameObjects to skip from the top of hierarchy;
        /// otherwise how many generations to include in the search upwards from <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/>.
        /// </summary>
        public int Generations { get; set; }

        /// <summary>
        /// <see cref="FamilyComponentAttribute"/>
        /// </summary>
        /// <param name="generationsBackwards"> <see cref="Generations"/></param>
        public FamilyComponentAttribute(int generationsBackwards = 1) : base(0)
        {
            Generations = generationsBackwards;
        }

        /// <summary>
        /// <see cref="FamilyComponentAttribute"/>
        /// </summary>
        /// <param name="fromRoot"> <see cref="FromRoot"/></param>
        /// <param name="generationOffset"> <see cref="Generations"/></param>
        public FamilyComponentAttribute(bool fromRoot, int generationOffset = 0) : base(0)
        {
            Generations = generationOffset;
            FromRoot = fromRoot;
        }
    }

    /// <summary>
    /// Reference point for <see cref="ReferenceComponentAttribute"/>, <see cref="ComponentAttribute"/> and <see cref="DependencyAttribute.Of"/>.
    /// </summary>
    protected class ReferencePointAttribute : PropertyAttribute { }

    /// <summary>
    /// Do not use it directly, it's just base class for all of GameObject dependency attributes.
    /// </summary>
    protected class GameObjectDependencyAttribute : DependencyAttribute {
        /// <summary>
        /// Name of the GameObject to find.
        /// </summary>
        protected virtual string Named { get; set; }

        /// <summary>
        /// Gets <see cref="Named"/>.
        /// </summary>
        /// <returns> <see cref="Named"/></returns>
        public string GetName()
        {
            return Named;
        }

        /// <summary>
        /// <see cref="GameObjectDependencyAttribute"/>
        /// </summary>
        /// <param name="generationOffset"> <see cref="DependencyAttribute.Offset"/></param>
        public GameObjectDependencyAttribute(int generationOffset = 0) : base(generationOffset) { }
    }
    /// <summary>
    /// Finds the <see cref="Named"/> GameObject in children or gets one by <see cref="Index">childIndex</see>.
    /// </summary>
    protected class ChildAttribute : GameObjectDependencyAttribute
    {
        /// <summary>
        /// Index of a child to get as the dependency.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets a child by its <see cref="Index"/>.
        /// </summary>
        /// <param name="childIndex"> <see cref="Index"/></param>
        public ChildAttribute(int childIndex = 0) : base(0)
        {
            Index = childIndex;
        }

        /// <summary>
        /// Finds the GameObject by <c>name</c> in children of <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/>.
        /// </summary>
        /// <param name="name"> Name of the GameObject to find.</param>
        public ChildAttribute(string name) : base(0)
        {
            Named = name;
            Index = -1;
        }
    }
    /// <summary>
    /// Finds the <see cref="Named"/> GameObject in parents or gets one by <see cref="Index">parentIndex</see>.
    /// </summary>
    protected class ParentAttribute : GameObjectDependencyAttribute
    {
        /// <summary>
        /// Gets a parent by its <c>parentIndex</c>.
        /// </summary>
        /// <param name="parentIndex"> Index of a parent to get as the dependency. (0 = direct parent)</param>
        public ParentAttribute(int parentIndex = 0) : base(parentIndex) { }
        /// <summary>
        /// Finds the GameObject by <c>name</c> in parents of <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/>.
        /// </summary>
        /// <param name="name"> Name of the GameObject to find.</param>
        public ParentAttribute(string name) : base(0)
        {
            Named = name;
        }
    }
    /// <summary>
    /// Finds GameObject by <see cref="Of">name</see> searching in all of them
    /// but starting from [ReferencePoint]s then <c>this.gameObject</c>, its children, siblings, children of siblings, then goes to a parent and repeats the algorythm.
    /// Then <see cref="DependencyAttribute.Offset"/> is applied to found GameObject.
    /// </summary>
    /// <seealso cref="ReferencePointAttribute"/>
    protected class ReferenceAttribute : GameObjectDependencyAttribute
    {
        /// <summary>
        /// Name of GameObject to find and inject as dependency.
        /// </summary>
        protected override string Named { get => Of; set => Of = value; }

        /// <summary>
        /// <see cref="ReferenceAttribute"/>
        /// </summary>
        /// <param name="name"> Name of GameObject to find and inject as dependency.</param>
        public ReferenceAttribute(string name) : base(0)
        {
            Of = name;
        }
    }
    /// <summary>
    /// Gets root GameObject of <c>this</c>; or finds one by <c>name</c> searching in scene roots and then from root (-<see cref="FromTop"/>) down to <c>this</c>.
    /// By <c>this</c> you should understand <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/>.
    /// </summary>
    protected class RootAttribute : GameObjectDependencyAttribute
    {
        /// <summary>
        /// Level of the hierarchy to treat as root. (0 = actual root)
        /// </summary>
        public int FromTop { get; set; }
        /// <summary>
        /// Gets root GameObject as the dependency.
        /// </summary>
        /// <param name="toSkipFromTop"> Level of the hierarchy to treat as root. (0 = actual root)</param>
        public RootAttribute(int toSkipFromTop = 0) : base(0)
        {
            FromTop = toSkipFromTop;
        }
        /// <summary>
        /// Finds GameObject by <c>name</c> searching from root (-<see cref="FromTop"/>) down to <c>this</c>.
        /// </summary>
        /// <remarks>
        /// By <c>this</c> you should understand <c>this.gameObject</c> or <see cref="Of"/> & <see cref="Offset"/>.
        /// </remarks>
        /// <param name="name"> Name of GameObject to find and inject as dependency.</param>
        public RootAttribute(string name) : base(0)
        {
            Named = name;
        }
    }
    /// <summary>
    /// Finds the <see cref="Named"/> GameObject in siblings or gets one by <see cref="Index">siblingIndex</see>.
    /// </summary>
    /// <remarks>
    /// <c>this.gameObject</c> is also treated as sibling.
    /// </remarks>
    protected class SiblingAttribute : GameObjectDependencyAttribute
    {
        /// <summary>
        /// Index of a sibling to get as the dependency. (<c>this.gameObject</c> is included)
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Get the GameObject by <see cref="Index"/>.
        /// </summary>
        /// <param name="siblingIndex"> Index of a sibling to get as the dependency.</param>
        public SiblingAttribute(int siblingIndex) : base(0)
        {
            Index = siblingIndex;
        }
        /// <summary>
        /// Find the GameObject by <c>name</c> in siblings.
        /// </summary>
        /// <param name="name"> Name of the GameObject to find.</param>
        public SiblingAttribute(string name) : base(0)
        {
            Named = name;
            Index = -1;
        }
    }

    private Transform GetOffsetTransform(Transform t, int offset)
    {
        if (t == null)
            return null;

        if (offset > 0)
        {
            for (int i = 0; i < offset; i++)
            {
                t = t.parent;
                if (t == null)
                    break;
            }
        }
        else
        {
            for (int i = offset; i < 0; i++)
            {
                if (t.childCount == 0)
                    break;
                t = t.GetChild(0);
            }
        }
        return t;
    }

    private object GetReferenceComponent(List<GameObject> references, MethodInfo method)
    {
        if(references.Count == 0)
        {
            LogWarning("There are no [ReferencePoint] GameObjects to search in for the [ReferenceComponent] in " + this + ". For automatic search use [Component] instead.");
            return null;
        }

        object component = null;
        foreach (var go in references)
        {
            Transform target;
            try
            {
                target = go.transform;
            }
            catch
            {
                continue;
            }

            component = method.Invoke(target, new Type[0]);
            if (component != null)
                break;
        }
        return component;
    }

    private Transform FindGameObjectInChildren(Transform parent, string name)
    {
        // Top search
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);

            if (child.gameObject.name.Equals(name))
                return child;
        }

        // Deep search
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);

            Transform result = FindGameObjectInChildren(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    private Transform FindGameObjectIn(Transform parent, string name)
    {
        if (parent.gameObject.name.Equals(name))
            return parent;

        return FindGameObjectInChildren(parent, name);
    }

    private class SceneOrDdols
    {
        private Scene? scene;

        public SceneOrDdols(GameObject go)
        {
            try
            {
                scene = go.scene;
            }
            catch
            {
                scene = null;
            }
        }

        public bool IsScene()
        {
            return scene.HasValue;
        }

        public Scene GetScene()
        {
            return scene.Value;
        }

        public GameObject[] GetRootGameObjects()
        {
            if (IsScene())
                return GetRoots(GetScene()).ToArray();
            else
                return GetDontDestroyOnLoadObjects().ToArray();
        }

        public int GetRootCount()
        {
            if (IsScene())
                return GetScene().rootCount;
            else
                return GetDontDestroyOnLoadObjects().Count;
        }

        public override string ToString()
        {
            if (IsScene())
                return scene.Value.name;
            else
                return "DDOLs Scene";
        }

        public bool Equals(Scene scene2)
        {
            if (scene.HasValue)
                return scene.Value == scene2;
            else
                return false;
        }
    }

    private List<Scene> GetScenes(Transform startPoint)
    {
        var list = new List<Scene>();
        var firstScene = new SceneOrDdols(startPoint?.gameObject);
        if (firstScene.IsScene())
            list.Add(firstScene.GetScene());

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (!firstScene.Equals(scene))
                list.Add(scene);
        }

        return list;
    }

    /// <summary>
    /// Returns scene.GetRootGameObjects() sorted alphabetically.
    /// </summary>
    private static List<GameObject> GetRoots(Scene scene)
    {
        var list = new List<GameObject>(scene.GetRootGameObjects());
        list.Sort((x, y) => string.Compare(x.name, y.name));
        return list;
    }

    private GameObject FindGameObject(string name, List<Scene> delayedScenes)
    {
        Transform startPoint = this.transform;
        // Children search
        Transform result = FindGameObjectIn(startPoint, name);
        if (result != null)
            return result.gameObject;

        // Climbing search
        while (startPoint.parent != null)
        {
            var toSkip = startPoint;
            startPoint = startPoint.parent;

            // Check parent
            if (toSkip.gameObject.name.Equals(name))
                return toSkip.gameObject;

            // Search in parent's siblings (Top)
            for (int i = 0; i < startPoint.childCount; i++)
            {
                var child = startPoint.GetChild(i);

                if (child != toSkip)
                {
                    if (child.gameObject.name.Equals(name))
                        return child.gameObject;
                }
            }

            // Search in children of the siblings (Deep)
            for (int i = 0; i < startPoint.childCount; i++)
            {
                var child = startPoint.GetChild(i);

                if (child != toSkip)
                {
                    result = FindGameObjectInChildren(startPoint, name);

                    if (result != null)
                        return child.gameObject;
                }
            }
        }

        // Search in roots
        // Check own root
        if (startPoint.name.Equals(name))
            return startPoint.gameObject;

        var scenes = GetScenes(startPoint);

        // Top search
        foreach (var scene in scenes)
        {
            var roots = GetRoots(scene);
            foreach (var r in roots)
            {
                if (r.name.Equals(name))
                    return r;
            }

            if (!scene.isLoaded)
                delayedScenes.Add(scene);
        }
        var ddols = GetDontDestroyOnLoadObjects();
        foreach (var go in ddols)
        {
            if (go.name.Equals(name))
                return go;
        }

        // Deep search
        foreach (var scene in scenes)
        {
            var roots = GetRoots(scene);
            foreach (var r in roots)
            {
                if(r.transform != startPoint)
                { 
                    result = FindGameObjectInChildren(r.transform, name);

                    if (result != null)
                        return result.gameObject;
                }
            }
        }
        foreach (var go in ddols)
        {
            result = FindGameObjectInChildren(go.transform, name);

            if (result != null)
                return result.gameObject;
        }

        return null;
    }

    private GameObject FindGameObject(DependencyAttribute a, FieldInfo f, List<GameObject> references, out bool isDelayed)
    {
        isDelayed = false;
        var name = a.Of;
        GameObject result = null;
        var delayedScenes = new List<Scene>();

        if (references.Count != 0)
        {
            var field = this.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (field != null && field.GetCustomAttribute(typeof(ReferencePointAttribute)) != null)
            {
                var reference = field.GetValue(this);
                try
                {
                    if (reference != null)
                    {
                        if (reference is GameObject go)
                            return go;
                        else if (reference is Transform t)
                            return t.gameObject;
                    }
                }
                catch
                {
                    Debug.LogError("The " + field + " field in " + this + " is most likely unassigned.");
                }
            }
            else
            {
                result = FindGameObjectIn(references, name);

                if (result == null)
                    result = FindGameObject(a.Of, delayedScenes);
            }
        }
        else
        {
            result = FindGameObject(a.Of, delayedScenes);
        }

        if (result == null)
        {
            if (delayedScenes.Count != 0)
            {
                isDelayed = true;
                StartCoroutine(FindGameObjectInScenes(delayedScenes, a, f, references));
            }
            else
            {
                if (a.Optional)
                    Log("No GameObject named \"" + a.Of + "\" was found for [" + a + "] " + f + " in " + this + " but it's optional.", true);
                else
                {
                    LogWarning("No GameObject named \"" + a.Of + "\" was found for [" + a + "] " + f + " in " + this + ". Creating one...");
                    return new GameObject(a.Of);
                }
            }
        }

        return result;
    }

    private GameObject FindGameObjectIn(IEnumerable<GameObject> list, string name)
    {
        foreach(var go in list)
        {
            try
            {
                if (go.name.Equals(name))
                    return go;
            }
            catch
            {
                continue;
            }
        }
        return null;
    }

    private IEnumerator InitDelayedSingleton(Type singletonType, FieldInfo f)
    {
        // Wait for scenes to load
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            while (!scene.isLoaded)
                yield return null;
        }

        // Init field
        var component = singletonType.GetProperty("Instance").GetValue(null);
        if (component != null)
        { 
            f.SetValue(this, component);
            Log("The [GlobalComponent] " + f + " field in " + this + " has been initialized with a delay. Don't use it in Start() & Awake()!");
        }
    }

    private IEnumerator WaitForScenesToLoad(IEnumerable<Scene> scenes)
    {
        foreach (var scene in scenes)
        {
            // Wait for scene to load
            while (!scene.isLoaded)
                yield return null;
        }
    }

    private GameObject FindGameObjectInScenes(IEnumerable<Scene> scenes, string name, bool deepSearch = true)
    {
        // Top
        foreach (var scene in scenes)
        {
            var roots = GetRoots(scene);
            foreach (var r in roots)
            {
                if (r.transform.name.Equals(name))
                    return r;
            }
        }

        if (deepSearch)
        { 
            // Deep
            foreach (var scene in scenes)
            {
                var roots = GetRoots(scene);
                foreach (var r in roots)
                {
                    var result = FindGameObjectInChildren(r.transform, name);
                    if (result != null)
                        return result.gameObject;
                }
            }
        }
        return null;
    }

    private IEnumerator FindGameObjectInScenes(IEnumerable<Scene> scenes, DependencyAttribute a, FieldInfo f, List<GameObject> references = null)
    {
        yield return StartCoroutine(WaitForScenesToLoad(scenes));
        var go = FindGameObjectInScenes(scenes, a.Of);

        if (go == null)
        {
            if (a.Optional)
            {
                Log("No GameObject named \"" + a.Of + "\" was found for [" + a + "]" + " " + f + " in " + this + " but it's optional.", true);
                yield break;
            }
            else
            {
                LogWarning("No GameObject named \"" + a.Of + "\" was found for [" + a + "]" + " " + f + " in " + this + ". Creating one...");
                go = new GameObject(a.Of);
            }
        }
        
        Log("The [" + a + "(Of = \"" + a.Of + "\")] " + f + " field in " + this + " is initialized with a delay. Don't use it in Start() & Awake()!");
        var startPoint = GetOffsetTransform(go.transform, a.Offset);
        if (a is GameObjectDependencyAttribute oa)
            InitObjectField(oa, f, startPoint);
        else if(a is ComponentDependencyAttribute ca)
            InitComponentField(ca, this.GetType(), f, startPoint, references);
    }

    private MethodInfo GetTopMethod(Type type)
    {
        return this.GetType().GetMethod("GetComponent", new Type[0]).MakeGenericMethod(type);
    }

    private MethodInfo GetDeepMethod(Type type)
    {
        return this.GetType().GetMethod("GetComponentInChildren", new Type[0]).MakeGenericMethod(type);
    }

    private class ToSkip : MonoBehaviour { }

    private bool HasToSkip(Transform t)
    {
        if (t.GetComponent<ToSkip>() == null)
            return false;
        else
            return true;
    }

    private object FindComponentInChildren(Transform parent, MethodInfo method)
    {   
        // Top search
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);

            if(!HasToSkip(child))
            {
                var target = method.Invoke(child, new Type[0]);
                if (target != null)
                    return target;
            }
        }

        // Deep search
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);

            if (!HasToSkip(child))
            {
                var result = FindComponentInChildren(child, method);
                if (result != null)
                    return result;
            }
        }
        return null;
    }

    private object FindComponentIn(Transform parent, MethodInfo method)
    {
        if (HasToSkip(parent))
            return null;

        var target = method.Invoke(parent, new Type[0]);
        if (target != null)
            return target;

        return FindComponentInChildren(parent, method);
    }

    private IEnumerator FindComponentInScenes(IEnumerable<Scene> scenes, FieldInfo f, bool deepSearch, bool isOptional, GameObject toSkip = null)
    {
        MethodInfo method = deepSearch ? GetDeepMethod(f.FieldType) : GetTopMethod(f.FieldType);

        if (toSkip != null)
        {
            toSkip.AddComponent<ToSkip>();
        }

        object target = null;
        foreach (var scene in scenes)
        {
            // Wait for scene to load
            while (!scene.isLoaded)
                yield return null;

            // Find component
            var roots = GetRoots(scene);
            foreach (var r in roots)
            {
                if (toSkip == r)
                    continue;

                if(!deepSearch || toSkip == null)
                {
                    target = method.Invoke(r.transform, new Type[0]);
                }
                else
                {
                    target = FindComponentIn(r.transform, GetTopMethod(f.FieldType));
                }

                if (target != null)
                    goto outLoops;
            }
        } outLoops:

        if (toSkip != null)
        {
            DestroyImmediate(toSkip.GetComponent<ToSkip>());
        }

        if (target == null && toSkip != null && deepSearch)
            target = GetComponentOnlyInChildren(toSkip.transform, method);

        if(f.GetValue(this) == null || !deepSearch)
        { 
            if (target != null)
            {
                f.SetValue(this, target);
                Log("The " + f + " field in " + this + " has been initialized with a delay. Don't use it in Start() & Awake()!");
            }
            else if(f.GetValue(this) == null)
            {
                if (isOptional)
                    Log("Instance of " + f + " not found for " + this + " but it's optional.", true);
                else
                {
                    LogWarning("Instance of " + f + " not found for " + this + ". Creating it with a delay... Don't use it in Start() & Awake()!");
                    CreateComponent(new GlobalComponentAttribute(), f, null, null);
                }
            }
        }
    }

    private object FindInRoots(FieldInfo f, bool deepSearch, bool isOptional, GameObject startPoint, out bool isDelayed, bool skip = false)
    {
        MethodInfo method = deepSearch ? GetDeepMethod(f.FieldType) : GetTopMethod(f.FieldType);

        Transform preParent = null;
        int siblingIndex = -1;
        if (skip && startPoint != null)
        {
            // Move object to skip to the top of the hierarchy for a while
            preParent = startPoint.transform.parent;
            siblingIndex = startPoint.transform.GetSiblingIndex();
            startPoint.transform.SetParent(null);
        }

        isDelayed = false;
        var delayedScenes = new List<Scene>();
        object result = null;
        var scenes = GetScenes(startPoint?.transform);
        foreach (var scene in scenes)
        {
            var roots = GetRoots(scene);
            foreach (var r in roots)
            {
                if (skip && startPoint == r)
                    continue;

                result = method.Invoke(r.transform, new Type[0]);
                if (result != null)
                    goto outLoops;
            }

            if (!scene.isLoaded)
            {
                delayedScenes.Add(scene);
            }
        } outLoops:

        if (result == null)
        {
            var ddols = GetDontDestroyOnLoadObjects();
            foreach (var go in ddols)
            {
                if (skip && startPoint == go)
                    continue;

                result = method.Invoke(go.transform, new Type[0]);
                if (result != null)
                    break;
            }
        }

        if (skip && startPoint != null)
        {
            // Restore the object's original position
            startPoint.transform.SetParent(preParent);
            startPoint.transform.SetSiblingIndex(siblingIndex);
        }

        if (result == null)
        {
            if (delayedScenes.Count != 0)
            {
                isDelayed = true;
                StartCoroutine(FindComponentInScenes(delayedScenes, f, deepSearch, isOptional, skip ? startPoint : null));
            }
            else if(skip && startPoint != null && deepSearch)
            {
                result = GetComponentOnlyInChildren(startPoint.transform, method);
            }
        }

        return result;
    }

    private object GetComponentOnlyInChildren(Transform parent, MethodInfo method)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var result = method.Invoke(child, new Type[0]);
            if (result != null)
                return result;
        }
        return null;
    }

    private object GetSingletonComponent(FieldInfo f, out bool isDelayed)
    {
        isDelayed = false;
        try 
        { 
            var fieldType = f.FieldType;
            var singletonType = typeof(MonoBehaviourSingleton<>).MakeGenericType(fieldType);
            if (fieldType.IsSubclassOf(singletonType))
            {
                var component = singletonType.GetProperty("Instance").GetValue(null);
                if (component == null) // It can be null only if some scene is still loading...
                {
                    StartCoroutine(InitDelayedSingleton(singletonType, f)); //so wait for scenes to load and then init the field.
                    isDelayed = true;
                }
                return component;
            }
        }
        catch
        {
            return null;
        }
        return null;
    }

    private void InitComponentField(ComponentDependencyAttribute a, Type t, FieldInfo f, Transform startPoint, List<GameObject> references)
    {
        object component = null;
        if (a is ChildComponentAttribute cc)
        {
            if (startPoint != null)
            {
                var deepMethod = GetDeepMethod(f.FieldType);
                if (cc.SkipItself)
                {
                    component = GetComponentOnlyInChildren(startPoint, deepMethod);
                }
                else
                {
                    component = deepMethod.Invoke(startPoint, new Type[0]);
                }
            }
            else
            {
                component = FindInRoots(f, true, a.Optional, transform.gameObject, out bool isDelayed);
                if (isDelayed)
                    return;
            }
        }
        else if (a is FamilyComponentAttribute fc)
        {
            var prePoint = startPoint;
            if(fc.FromRoot)
                startPoint = GetRoot(startPoint, fc.Generations);
            else
                startPoint = GetOffsetTransform(startPoint, fc.Generations);

            if (startPoint != null)
            {
                var deepMethod = GetDeepMethod(f.FieldType);
                if (fc.SkipItself)
                {
                    // Get the object to skip out of family for a while
                    var preParent = prePoint.parent;
                    var siblingIndex = prePoint.GetSiblingIndex();
                    prePoint.SetParent(null);

                    // Search in family
                    if (startPoint != prePoint)
                        component = deepMethod.Invoke(startPoint, new Type[0]);

                    // Search in children of skiped object
                    if (component == null && startPoint.root != prePoint)
                        component = GetComponentOnlyInChildren(prePoint, deepMethod);

                    // Restore the object to the family
                    prePoint.SetParent(preParent);
                    prePoint.SetSiblingIndex(siblingIndex);
                }
                else
                {
                    component = deepMethod.Invoke(startPoint, new Type[0]);
                }
            }
            else
            {
                component = FindInRoots(f, true, a.Optional, prePoint?.gameObject, out bool isDelayed, fc.SkipItself);
                if (isDelayed)
                    return;
            }
        }
        else if (a is ParentComponentAttribute pc)
        {
            if (pc.SkipItself)
            {
                startPoint = startPoint?.parent;
            }
            if (startPoint != null)
            {
                component = t.GetMethod("GetComponentInParent", new Type[0]).MakeGenericMethod(f.FieldType).Invoke(startPoint, new Type[0]);
            }
        }
        else if (a is GlobalComponentAttribute)
        {
            if (a.Of != null && startPoint != null)
            {
                var method = GetTopMethod(f.FieldType);
                component = method.Invoke(startPoint, new Type[0]);
            }

            if (component == null)
            {
                // Is of singleton type?
                if (!a.Optional) // Singleton instance is always there (if not, it's created), so for optional don't even try
                {
                    component = GetSingletonComponent(f, out bool isDelayed);
                    if (isDelayed)
                        return;
                }
                
                if(component == null)
                {
                    //... no, it's not (or optional==true) so
                    // First search in roots itself
                    component = FindInRoots(f, false, a.Optional, startPoint.gameObject, out bool isDelayed);

                    // Then in their children
                    if (component == null)
                    {
                        component = FindInRoots(f, true, a.Optional, startPoint.gameObject, out bool isDelayed2);

                        if (component == null && (isDelayed || isDelayed2))
                            return;
                    }
                }
            }
        }
        else if (a is SiblingComponentAttribute sc)
        {
            Transform parent = startPoint?.parent;
            if (parent != null)
            {
                var method = GetTopMethod(f.FieldType);
                if (sc.SkipItself)
                {
                    // Get the object to skip out of family for a while
                    var siblingIndex = startPoint.GetSiblingIndex();
                    startPoint.SetParent(null);

                    // Search in siblings
                    component = GetComponentOnlyInChildren(parent, method);

                    // Restore the object to the family
                    startPoint.SetParent(parent);
                    startPoint.SetSiblingIndex(siblingIndex);
                }
                else
                {
                    component = GetComponentOnlyInChildren(parent, method);
                }
            }
            else
            {
                component = FindInRoots(f, false, a.Optional, startPoint?.gameObject, out bool isDelayed, sc.SkipItself);
                if (isDelayed)
                {
                    return;
                }
            }
        }
        else if (a is OwnComponentAttribute)
        {
            if (startPoint != null)
            { 
                var method = GetTopMethod(f.FieldType);
                component = method.Invoke(startPoint, new Type[0]);
            }
        }
        else if (a is ReferenceComponentAttribute rc)
        {
            var method = rc.DeepSearch ? GetDeepMethod(f.FieldType) : GetTopMethod(f.FieldType);
            if (a.Of != null)
            {
                if(startPoint!=null)
                    component = method.Invoke(startPoint, new Type[0]);
            }
            else
            {
                component = GetReferenceComponent(references, method);
            }
        }
        else if (a is ComponentAttribute)
        {
            // Is it singleton?
            if (!a.Optional && a.Of == null)
            {
                component = GetSingletonComponent(f, out bool isDelayed);
                if (isDelayed)
                    return;
            }

            // If not, countinue searching
            var topMethod = GetTopMethod(f.FieldType);
            if (component == null && a.Of == null && references.Count != 0)
            {
                component = GetReferenceComponent(references, topMethod);       
            }

            if (component == null && startPoint != null)
            {
                var deepMethod = GetDeepMethod(f.FieldType);
                component = deepMethod.Invoke(startPoint, new Type[0]);
                    
                while (component == null)
                {
                    var toSkip = startPoint;

                    var parent = startPoint.parent;
                    if (parent == null)
                        break;
                    else
                        startPoint = parent;

                    component = topMethod.Invoke(toSkip, new Type[0]);

                    if (component == null)
                        component = GetComponentOnlyInChildren(startPoint, topMethod);

                    if (component == null)
                    { 
                        for (int i = 0; i < startPoint.childCount; i++)
                        {
                            var child = startPoint.GetChild(i);

                            if (child != toSkip)
                                component = GetComponentOnlyInChildren(child, deepMethod);

                            if (component != null)
                                break;
                        }
                    }
                }
            }

            if (component == null)
            {
                if (startPoint != null)
                {
                    component = topMethod.Invoke(startPoint, new Type[0]);
                }
                if (component == null)
                {
                    component = FindInRoots(f, true, a.Optional, startPoint?.gameObject, out bool isDelayed, startPoint!=null);
                    if (isDelayed)
                        return;
                }
            }
        }

        if (component == null)
        {
            if (a.Optional)
                Log("Instance of [" + a + "]" + " " + f + " not found for " + this + " but it's optional.", true);
            else
            {
                LogWarning("Instance of [" + a + "]" + " " + f + " not found for " + this + ". Creating one...");
                CreateComponent(a, f, startPoint, references);
            }
        }
        else
        {
            f.SetValue(this, component);
        }
    }

    private Transform GetChild(Transform parent, int index)
    {
        if (index < 0 || index >= parent.childCount)
        {
            return null;
        }
        return parent.GetChild(index);
    }

    private Transform GetRoot(Transform startPoint, int offsetFromTop)
    {
        int toRoot = 0;
        Transform root = startPoint;
        while (root.parent != null)
        {
            toRoot++;
            root = root.parent;
        }

        if (offsetFromTop != 0)
        {
            root = startPoint;
            toRoot -= Mathf.Abs(offsetFromTop);
            root = GetOffsetTransform(root, toRoot);
        }

        return root;
    }

    private Transform GetTransformByStartPoint(GameObjectDependencyAttribute a, FieldInfo f, Transform startPoint)
    {
        if (startPoint == null)
            return null;

        Transform target = null;
        if (a is ParentAttribute)
        {
            target = startPoint.parent;
        }
        else if (a is ChildAttribute ca)
        {
            target = GetChild(startPoint, ca.Index);

            if (target == null)
            {
                string msg = "ChildIndex of [" + a + "(" + ca.Index + ")]" + " " + f + " in " + this + " is out of rage of " + startPoint.name + "'s children.";
                if (a.Optional)
                    Log(msg, true);
                else
                    LogWarning(msg);
            }
        }
        else if (a is SiblingAttribute sa)
        {
            var parent = startPoint.parent;
            if (parent != null)
            { 
                target = GetChild(parent, sa.Index);

                if (target == null)
                {
                    string msg = "ChildIndex of [" + a + "(" + sa.Index + ")]" + " " + f + " in " + this + " is out of rage of " + parent.name + "'s children.";
                    if (a.Optional)
                        Log(msg, true);
                    else
                        LogWarning(msg);
                }
            }
            else
            {
                var scene = new SceneOrDdols(startPoint.gameObject);
                GameObject[] roots = scene.GetRootGameObjects();
                var index = sa.Index;
                if (index < 0 || index >= roots.Length)
                {
                    string msg = "ChildIndex of [" + a + "(" + index + ")]" + " " + f + " in " + this + " is out of rage of " + scene + "'s roots.";
                    if (a.Optional)
                        Log(msg, true);
                    else
                        LogWarning(msg);
                }
                else
                {
                    target = roots[index].transform;
                }
            }
        }
        else if (a is ReferenceAttribute)
        {
            target = startPoint;
        }
        else if (a is RootAttribute ra)
        {
            target = GetRoot(startPoint, ra.FromTop);
        }
        return target;
    }

    private IEnumerator FindGameObjectInRoots(GameObjectDependencyAttribute a, IEnumerable<Scene> scenes, FieldInfo f, bool isOptional, string name, bool deepSearch)
    {
        yield return StartCoroutine(WaitForScenesToLoad(scenes));
        var go = FindGameObjectInScenes(scenes, name, deepSearch);

        if (go == null)
        {
            if (f.GetValue(this) != null)
            {
                yield break;
            }

            if (isOptional)
            {
                Log("No GameObject named \"" + name + "\" was found for [" + a + "] " + f + " in " + this + " but it's optional.", true);
                yield break;
            }
            else
            {
                LogWarning("No GameObject named \"" + name + "\" was found for [" + a + "] " + f + " in " + this + ". Creating one...");
                go = new GameObject(name);
            }            
        }

        if (f.FieldType == typeof(GameObject))
            f.SetValue(this, go);
        else // Transform
            f.SetValue(this, go.transform);

        Log("The [" + a + "(Named = \"" + name + "\")] " + f + " field in " + this + " has been initialized with a delay. Don't use it in Start() & Awake()!");
    }

    private Transform FindGameObjectInRoots(GameObjectDependencyAttribute a, FieldInfo f, bool isOptional, string name, bool deepSearch, out bool isDelayed)
    {
        isDelayed = false;

        var delayedScenes = new List<Scene>();
        var scenes = GetScenes(transform);
        foreach (var scene in scenes)
        {
            var roots = GetRoots(scene);
            foreach(var r in roots)
            {
                if (deepSearch)
                {
                    var result = FindGameObjectInChildren(r.transform, name);
                    if (result != null)
                        return result;
                }
                else
                {
                    if (r.transform.name.Equals(name))
                        return r.transform;
                }
            }
            
            if (!scene.isLoaded)
                delayedScenes.Add(scene);
        }

        if (delayedScenes.Count != 0)
        {
            isDelayed = true;
            StartCoroutine(FindGameObjectInRoots(a, delayedScenes, f, isOptional, name, deepSearch));
        }

        return null;
    }

    private Transform GetTransformByName(GameObjectDependencyAttribute a, FieldInfo f, Transform startPoint, out bool isDelayed)
    {
        isDelayed = false;
        var name = a.GetName();
        if (a is ParentAttribute)
        {
            if(startPoint != null)
            { 
                while (startPoint.parent != null)
                {
                    startPoint = startPoint.parent;
                    if (startPoint.name.Equals(name))
                        return startPoint;
                }
            }
            return null;
        }
        else if (a is ChildAttribute)
        {
            if (startPoint != null)
            {
                return FindGameObjectInChildren(startPoint, name);
            }
            else
            {
                return FindGameObjectInRoots(a, f, a.Optional, name, true, out isDelayed);
            }
        }
        else if (a is SiblingAttribute)
        {
            startPoint = startPoint?.parent;
            if (startPoint != null)
            {
                for(int i = 0; i<startPoint.childCount; i++)
                {
                    var child = startPoint.GetChild(i);
                    if (child.name.Equals(name))
                        return child;
                }
                return null;
            }
            else
            {
                return FindGameObjectInRoots(a, f, a.Optional, name, false, out isDelayed);
            }
        }
        else if (a is ReferenceAttribute)
        {
            return startPoint;
        }
        else if (a is RootAttribute ra)
        {
            if (startPoint != null)
            {
                int toRoot = 0;
                var path = new List<Transform>();
                Transform root = startPoint;
                while (root.parent != null)
                {
                    toRoot++;
                    root = root.parent;
                }

                root = startPoint;
                toRoot -= Mathf.Abs(ra.FromTop);
                if (toRoot < 0)
                {
                    root = GetOffsetTransform(startPoint, toRoot);
                }
                for (int i = 0; i < Mathf.Abs(toRoot); i++)
                {
                    path.Add(root);
                    if (root.parent == null)
                        break;
                    root = root.parent;
                }
                if (toRoot < 0)
                {
                    path.Reverse();
                    root = GetOffsetTransform(startPoint, toRoot);
                }

                if (root.name.Equals(name))
                    return root;

                var sa = new SiblingAttribute(name);
                sa.Optional = ra.Optional;
                var result = GetTransformByName(sa, f, root, out isDelayed);
                if (result != null)
                    return result;

                for (int i = path.Count - 1; i>=0; i--)
                {
                    var t = path[i];
                    if (t.name.Equals(name))
                        return t;
                }
            }
            else if(ra.FromTop == 0)
            {
                return FindGameObjectInRoots(a, f, a.Optional, name, false, out isDelayed);
            }
        }
        return null;
    }

    private void InitObjectField(GameObjectDependencyAttribute a, FieldInfo f, Transform startPoint)
    {
        Transform target;
        if (a.GetName() == null)
        {
            target = GetTransformByStartPoint(a, f, startPoint);
        }
        else
        {
            target = GetTransformByName(a, f, startPoint, out bool isDelayed);
            if (isDelayed && !(a is RootAttribute))
                return;
        }

        if (target == null)
        {
            if (a.Optional)
                Log("Instance of [" + a + "]" + " " + f + " not found for " + this + " but it's optional.", true);
            else
            {
                LogWarning("Instance of [" + a + "]" + " " + f + " not found for " + this + ".");
                CreateGameObject(a, f, startPoint);
            }
        }
        else
        {
            if (f.FieldType == typeof(GameObject))
                f.SetValue(this, target.gameObject);
            else // Transform
                f.SetValue(this, target);
        }
    }

    private void CreateGameObject(GameObjectDependencyAttribute a, FieldInfo f, Transform startPoint)
    {
        if (startPoint == null)
            return;

        string name = a.GetName() ?? f.Name;
        Transform target = new GameObject(name).transform;

        if (a is ParentAttribute)
        {
            target.SetParent(startPoint.parent);
            startPoint.SetParent(target);
        }
        else if (a is ChildAttribute ca)
        {
            int children = startPoint.childCount;
            while (children < ca.Index)
            {
                new GameObject("Child" + (children+1)).transform.SetParent(startPoint);
                children++;
            }
                    
            target.SetParent(startPoint);
            if (ca.Index >= 0)
                target.SetSiblingIndex(ca.Index);
        }
        else if (a is SiblingAttribute sa)
        {
            var parent = startPoint.parent;
            int children = parent?.childCount ?? new SceneOrDdols(startPoint.gameObject).GetRootCount();
            while (children < sa.Index)
            {
                var sibling = new GameObject("Sibling" + (children+1)).transform;
                sibling.SetParent(startPoint); // Sets scene... in case the parent was null
                sibling.SetParent(parent);
                children++;
            }

            target.SetParent(startPoint); // Sets scene of target to be the same as of startPoint
            target.SetParent(parent);
            if (sa.Index >= 0)
                target.SetSiblingIndex(sa.Index);
        }
        else if (a is RootAttribute ra)
        {
            if (ra.FromTop != 0)
            {
                int toRoot = 0;
                Transform root = startPoint;
                while (root.parent != null)
                {
                    toRoot++;
                    root = root.parent;
                }

                root = startPoint;
                toRoot -= Mathf.Abs(ra.FromTop);
                root = GetOffsetTransform(root, toRoot);

                target.SetParent(root.parent);
            }
            else
            {
                target.SetParent(startPoint);
                target.SetParent(null);
            }
        }

        if (f.FieldType == typeof(GameObject))
            f.SetValue(this, target.gameObject);
        else // Transform
            f.SetValue(this, target);
        Log("New GameObject called " + name + " has been created.", true);
    }

    private void CreateComponent(ComponentDependencyAttribute a, FieldInfo f, Transform startPoint, List<GameObject> references)
    {
        Type fType = f.FieldType;
        Component component = null;
        if (a is ChildComponentAttribute || a is FamilyComponentAttribute || a is SiblingComponentAttribute)
        {
            if (a is SiblingComponentAttribute)
            {
                startPoint = startPoint?.parent;
            }

            GameObject gm = new GameObject(fType.Name, fType);
            component = gm.GetComponent(fType);

            gm.transform.SetParent(startPoint);
        }
        else if (a is ParentComponentAttribute || a is GlobalComponentAttribute || a is OwnComponentAttribute || a is ComponentAttribute)
        {
            if (startPoint != null)
            {
                component = startPoint.gameObject.AddComponent(fType);
            }
            else
            {
                GameObject gm = new GameObject(fType.Name, fType);
                component = gm.GetComponent(fType);
            }
        }
        else if (a is ReferenceComponentAttribute)
        {
            if (a.Of != null || references.Count == 0)
            {
                if (startPoint != null)
                {
                    component = startPoint.gameObject.AddComponent(fType);
                }
                else
                {
                    GameObject gm = new GameObject(fType.Name, fType);
                    component = gm.GetComponent(fType);

                    // Set scene to be the same as this
                    gm.transform.SetParent(transform); 
                    gm.transform.SetParent(null);
                }
            }
            else
            {
                component = references[Mathf.Min(a.Offset, references.Count-1)].AddComponent(fType);
            }
        }

        f.SetValue(this, component);
        Log("New component [" + a + "]" + " " + f + " for " + this + " has been created.", true);
    }

    /// <summary>
    /// If you want to use Awake() in your script, overload this (with the <c>new</c> keyword) and call <c>base.Awake();</c>
    /// </summary>
    protected void Awake()
    {
        Type t = this.GetType();
        var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        List<GameObject> references = new List<GameObject>();
        foreach (var f in fields)
        {
            object[] attributes = f.GetCustomAttributes(typeof(GameObjectDependencyAttribute), true);
            if (attributes.Length > 1)
            {
                for (int i = 0; i < attributes.Length - 1; i++)
                    ((GameObjectDependencyAttribute)attributes[i]).Optional = true;
            }
            foreach (GameObjectDependencyAttribute a in attributes)
            {
                if (f.IsPublic && !f.IsNotSerialized)
                    LogWarning("You tried to inject a public (serialized) GameObject! (" + f + " in " + this + ") It should be private or marked with [NonSerialized], otherwise it may conflict with the Unity serializer and the dependency may not be injected.");

                if (f.GetValue(this) != null)
                    break;

                if (f.FieldType != typeof(GameObject) && f.FieldType != typeof(Transform))
                    Debug.LogError(a + " cannot be applied to " + f + " in " + this + " as it's not of GameObject/Transform type.");

                Transform startPoint = null;
                if (a.Of != null)
                {
                    startPoint = FindGameObject(a, f, references, out bool isDelayed)?.transform;
                    if (startPoint == null)
                        continue;
                }
                else
                {
                    startPoint = transform;
                }

                startPoint = GetOffsetTransform(startPoint, a.Offset);
                InitObjectField(a, f, startPoint);
                
            }

            if (f.GetCustomAttribute(typeof(ReferencePointAttribute)) != null)
            {
                if (f.FieldType == typeof(GameObject))
                    references.Add((GameObject)f.GetValue(this));
                else if (f.FieldType == typeof(Transform))
                    references.Add(((Transform)f.GetValue(this)).gameObject);
                else
                    Debug.LogError("ReferencePointAttribute cannot be applied to " + f + " in " + this + " as it's not of GameObject/Transform type.");
            }
        }

        foreach (var f in fields)
        {
            object[] attributes = f.GetCustomAttributes(typeof(ComponentDependencyAttribute), true);
            if (attributes.Length > 1)
            {
                for (int i = 0; i < attributes.Length - 1; i++)
                    ((ComponentDependencyAttribute)attributes[i]).Optional = true;
            }
            foreach (ComponentDependencyAttribute a in attributes)
            {
                if (f.GetValue(this) != null)
                    break;

                Transform startPoint = null;
                if (a.Of != null)
                {
                    startPoint = FindGameObject(a, f, references, out bool isDelayed)?.transform;
                    if(startPoint == null)
                        continue;
                }
                else
                {
                    startPoint = transform;
                }

                startPoint = GetOffsetTransform(startPoint, a.Offset);
                InitComponentField(a, t, f, startPoint, references);
            }
        }
    }
}
