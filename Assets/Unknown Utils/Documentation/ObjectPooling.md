# Pool Manager

The **Pool Manager** provides a simple way to manage object pooling in your project.

## Pool Setup

Drag and drop **Pool Manager** into your scene and here the choice is yours to use singleton or use service locator for Pool manager.
You can make the changes in **Pool Manager** script itself.

### How to setup pool objects

Make a prefab of object you want to pool and add **PoolObject** script to it.

In inspector of pool manager you can see **All Objects Pools Info**

Give id of pool in list and give prefab that you made. 
Parent is optional.

Initial size is on awake how many objects you want to spawn it's same for all pool

## Methods 

	

| Method                                         | Description                                          |
| ---------------------------------------------- | ---------------------------------------------------- |
| `PoolManager.GetPoolObject(string id);`        | Returns PoolObject componenet to relase later		|
| `poolObject.Release();`					     | To release the object to it's pool					|


## Example

```csharp
PoolObject poolObject = PoolManager.Instance.GetPoolObject("Obj1");

// and after the work is done just do 
poolObject.Release();
```
