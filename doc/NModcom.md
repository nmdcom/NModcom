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
TBD

## Time events and the event list
TBD

## State events
TBD

## Numerical integration in MODCOM
TBD

## Representation of time in MODCOM
TBD

## Implement a model using MODCOM
TBD

## Run a simulation with two component models
TBD

## Serialization of a MODCOM model
TBD

## Using dff.exe to run simulations
TBD

## Compiling MODCOM components with MONO
TBD

## The special case of data exchange between crop, soil water, and soil nitrogen models
TBD

