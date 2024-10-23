# A brief introduction to modeling and simulation
TBD

# The MODCOM framework for simulation

The MODCOM framework for simulation allows the linking of model implementations. MODCOM handles numerical integration, event handling, and communication between component models. MODCOM was first described by Hillyer et al. (2003). It has been used in the EU-funded project SEAMLESS (Van Evert & Lamaker, 2007). It has also provided inspiration for SIMPLACE (Enders et al., 2010; http://www.simplace.net) and BioMa (Donatelli et al., 2012). MODCOM can be considered to be an implementation of the Discrete Event Specification (DEVS) (Zeigler, 1976, Zeigler et al., 2000).

## Classes in MODCOM

The concept of MODCOM can be readily understood by considering the UML class diagram in the figure below, which shows the most important classes in the framework. Each class is briefly explained below. 

![Framework classes](images/modcom-class-diagram.svg "Class diagram of MODCOM")



### ISimEnv
The interface that defines a simulation environment. One, two, or more component models may be grouped in an ISimEnv (simulation environment). ISimEnv keeps track of time (start time and stop time for the simulation, as well as current simulation time). ISimEnv also provides methods to proceed with a simulation one step at a time (Step()) or run the simulation from beginning to end (Run()). ISimEnv has a method Add() which can be used to register model implementations that form part of the simulation. ISimEnv also maintains a list of ISimEvent instances (see below). Finally, ISimEnv has an instance of IIntegrator which provides numerical integration (in the diagram, EulerIntegrator class is shown as an implementation of IIntegrator).


### ISimObj
The interface that defines a component model. Component models have inputs (for parameters and external influences) and outputs (to learn about the state of the model). 

ISimObj instances may receive ISimEvent instances via a call to HandleEvent(). An ISimObj is expected to implement model-specific behaviour when it receives an ISimEvent. ISimObj implementations can register new ISimEvent�s with the ISimEnv where they are registered. The ISimEvent mechanism is therefore suffficient to implement both discrete-event and discrete-time simulations. See also �Time events and the event list1.3.

ISimObj instances that implement a model that is specified using differential equations request numerical integration services by implementing the IOdeProvider interface.

### IData
The interface that defines a data element to be exchanged between ISimObj�s. The outputs of an ISimObj hold an IData instance. The inputs of an ISimObj can hold a pointer to an IData instance. In this way, the output of one ISimObj can be used as in input for one or more other ISimObj instances.

### IIntegrator
Defines the behaviour of numerical integration classes.

### IOdeProvider
When this interface is implemented by a class that also implements ISimObj, it signals that this class (model) requires numerical integration services (it is a �provider� of Ordinary Differential Equations).

### ISimEvent
The  simulation environment maintains a list of objects that implement ISimEvent. This list is sorted by time (and priority, when two ISimEvent instances have the same time). When simulation time reaches the event time of an ISimEvent, appropriate action is taken.

### Example model: Predator
Predator is an example of the implementation of a model (here: growth of a predatory species of animal). Predator is not a framework class and is shown here only to clarify the relationship between a model implementation and the framework. Predator is a component model and therefore must implement ISimObj. It is also a differential equation-based model that requires the services of a numerical integrator; for this reason, it implements the IOdeProvider interface.



## Data exchange between component models
Component models can exchange data in the form of objects that implement the IData interface. The UML object below illustrates this.

![Data exchange via SimData objects](images/modcom-simdata-separate.svg ) 
![Data exchange via SimData objects](images/modcom-simdata-connected.svg )

In this Figure, expGrowth and env are instances of component models (ISimObj implementations). The env model instance specifies an output with name “T” and makes the data corresponding to that output available via an object called “T”. This object implements the IData interface and is shown in the Figure as holding the value “18.5”. The expGrowth model instance specifies an input, which incidentally has also name “T”. The input object “T” is capable of receiving a pointer to an IData object. 

## Time events and the event list
A MODCOM simulation is driven by time events. Time events are scheduled to occur at a certain time; at that time, the simulation object that is the target of the event is notified. Simulation objects respond to events in a way that makes sense to them. For example, integrator objects (objects that implement IIntegrator) perform an integration step. 

Events are implemented as classes and thus can encapsulate arbitrary amounts of information. This makes it possible that a crop object responds to “Harvest” events, for example by reducing its biomass; and that a soil object responds to “Tillage” events.

The event list is not fixed: during a simulation new events can be added, while scheduled events can be removed. Thus, an integration object may schedule additional integration events if the time step of numerical integration must be changed; and a farm management object may schedule irrigation events as they become necessary. 

![](images/time-events.png )

The following C# code snippet shows how to create a time event and add it to the event list:

```
TimeEvent e = new TimeEvent(this, target, time);
simenv.RegisterEvent(e);
```

## State events
MODCOM can work with state events. Imagine that the temperature in a greenhouse is rising slowly. When the temperature reaches a certain threshold, windows are opened to slow down or prevent a further rise in temperature. 

This means that as soon as the temperature reaches the threshold, the state of the system must be changed (windows opened). We do not know at which point in time the threshold will be reached. It is most likely that there will be an integration step at the beginning of which the threshold is not yet reached, while at the end of the step the threshold has been exceeded. 

In order to simulate this situation correctly, we must iteratively change the last integration step until we reach precisely the point in time at which the state event occurs; change the state of the system; and continue the simulation. In the figure, the integration step denoted by arrow 1 would result in an overshoot. Halving the time step still results in an overshoot (arrow 2). Halving the time step again results in undershoot (arrow 3). In this example, the final time step will be somewhere between arrows 2 and 3.

Creating and registering a state event is a bit more complicated than creating and registering a time event, because a state event must be linked with an algorithm to determine whether the condition it must detect has occurred.

![](images/state-events.png )

## Numerical integration in MODCOM
The UML sequence diagram below shows the method calls that are made during one integration step, using the Euler integrator. 

**TODO need more detail on this figure. What is Ode1 Ode2? Why is there only a small output in the left top of the figure? And why is a Step followed?**

![](images/numerical-integration-step.svg )

## Representation of time in MODCOM
In MODCOM time is always represented as a double precision floating point number. The interpretation of that number is up to the modeler. MODCOM will work correctly whether 0 represents 1 Jan 1970 00:00:00 UTC and the unit of time is a second (Unix-style), or whether 1 represents 1 Jan 1900 (Excel).

MODCOM provides a CalendarTime class which can be used to convert between simulation time and calendar time in a standard way. This class uses .NET DateTime class to represents dates and times with values ranging from 00:00:00 (midnight), January 1, 0001 through 11:59:59 P.M., December 31, 9999.


## Implement a model using MODCOM
TBD

## Run a simulation with two component models
TBD

