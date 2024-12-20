# MODCOM Tutorial <a name="Tutorial"></a>

This tutorial explains step-by-step how to use NMODCOM to create and run dynamic simulations.

1. [Working with the simulation environment  ](#SimEnv)
2. [Working with a simulation object ](#SimObj)
3. [UpdateableSimObj](#UpdateableSimObj)

## Working with the simulation environment <a name="SimEnv"></a>

[Back to top](#Tutorial) |
[View code for this section](https://github.com/nmdcom/NModcom/blob/main/NModcom.ExampleApp/SimEnvOnly.cs).

The first step in creating a MODCOM simulation is setting up the simulation environment. The simulation environment is a class that keeps track of the component models in a simulation and coordinates their interactions. The behaviour of the simulation environment is defined by the ISimEnv interface. The MODCOM framework provides SimEnv, a class that implements ISimEnv. 

You can set up a simulation by creating an instance of SimEnv. Don't forget to provide a start- and stoptime for the simulation. Once that's done, the simulation can be run by invoking the Run() method.

```
ISimEnv simenv = new SimEnv()
{
    StartTime = 0,
    StopTime = 5
};
simenv.Run();
```
MODCOM keeps track of time with a double-precision variable. The framework makes no assumption about the unit of time. It is up to the user to decide whether one unit is a second, a day, a nano-second, or a millenium - or any other measure of time!

MODCOM provides a convenience class that can transform to and from calendar date/time.

```
StartTime = CalendarTime.ToDouble(new DateTime(2024, 3, 11)),
StopTime = CalendarTime.ToDouble(new DateTime(2024, 3, 25))
```


## Working with a simulation object <a name="SimObj"></a>

[Back to top](#Tutorial) | [View code for this section](https://github.com/nmdcom/NModcom/blob/main/NModcom.ExampleApp/MyFirstSimObj.cs)

A simulation without any model code is of no use. We want to add at least one model to the simulation. This can be done by writing a class that implements the ISimObj interface, for example by inheriting from the utility class SimObj. This is shown in the code below.


```
    internal class MyFirstSimObj : SimObj
    {
        [Output("my output")]
        IData myOutput;

        public override void StartRun()
        {
            myOutput = new ConstFloatSimData(3.14);
        }

        public override void HandleEvent(ISimEvent simEvent)
        {
            myOutput.AsFloat += 1.0;
        }

    }

```

Several points need explaining.
First, the MyFirstSimObj model makes information about its state visible in two steps: by creating a field that implements the IData interface, and by declaring that this field is an output field:

 ```
        [Output("my output")]
        IData myOutput = new ConstFloatSimData(0);
```

Second, the MyFirstSimObj model overrides the StartRun() method declared by the ISimObj interface. StartRun() is called by the SimEnv before each simulation run. This is the right method to assign an initial value to the model's state. Here we create an instance of ConstFloatSimData which can hold a single floating point value.

```
public override void StartRun()
{
            myOutput.AsFloat = 3.14;
}
```

Third, we need to provide a mechanism for the model to change its state. A MODCOM simulation is entirely driven by events. We can make MyFirstSimObj respond to events by implementing HandleEvent() which is declared by ISimObj. 

In the code below, the state (output) of the model is incremented by 1 whenever an event is received. 

*Later we will see how events can be sent at specific times, to specific SimObj's, and how SimObj's can inspect events to determine how to respond*.

```
    public override void HandleEvent(ISimEvent simEvent)
    {
            myOutput.AsFloat += 1.0;
    }
```
Now that we have a model, we can use it in a simulation, simply by using the Add() method of ISimEnv.

```
   ISimEnv simenv = new SimEnv()
   {
       StartTime = 0,
       StopTime = 5
   };

   ISimObj mySimObj = new MyFirstSimObj()
   {
       Name = "Test"
   };

   simenv.Add(mySimObj);
```

The SimEnv maintains a list of SimObj's that have been added to it. We can refer to items in this either by index or by name. The same is true for the list of outputs maintained by a SimObj. That means that the two lines of code below will produce the same output.

```
    Console.WriteLine(simenv[0].Outputs[0].Data.AsFloat);
    Console.WriteLine(simenv["Test"].Outputs["my output"].Data.AsFloat);
```

Only one more thing to do. We need to register one or more events so that our model knows when to update its state.


```
   simenv.RegisterEvent(new TimeEvent(simenv, mySimObj, 0, 0, 1.5 ));
   simenv.RegisterEvent(new TimeEvent(simenv, mySimObj, 0, 0, 3.0 ));
```

When we run this simulation, it produces the following output:

```
[TODO insert output here]
```

## UpdateableSimObj <a name="UpdateableSimObj"></a>


[Back to top](#Tutorial) |
[Code for this section](https://github.com/nmdcom/NModcom/blob/main/NModcom.ExampleApp/DiscreteEvents.cs)


## OdeProvider

## Numerical integration
Euler and RK

## Connecting SimObj instances via SimData

## Time events

## State events