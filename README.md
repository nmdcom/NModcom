# NMODCOM
NMODCOM is a framework for discrete-event and continuous simulation as described
in the following paper:

Charles Hillyer, John Bolte, Frits van Evert, and Arjan Lamaker (2003). 

The ModCom modular simulation system. 

European Journal of Agronomy, vol. 18, pages 333-343. 

https://doi.org/10.1016/S1161-0301(02)00111-9.

Abstract: Simulation models of agro-ecological systems are typically written 
in a manner that precludes reusability of parts of the model without a 
significant amount of familiarity with and rewriting of existing code. Similarly, 
replacing a part of a model with a functionally equivalent part from another model 
is typically difficult. The objective of this study was to develop a method 
to enable the assembly of simulation models from previously and independently 
developed component models. Recent advances in software engineering have enabled 
the development of software applications from smaller parts (called components) 
on the basis of an abstract decomposition of the relevant domain (called a framework). 
Based on a requirements analysis of existing simulation models we developed the 
ModCom simulation framework. ModCom provides a set of interface specifications that 
describe components in a simulation. ModCom also provides implementations of the 
core simulation services. The framework interfaces use well-defined binary standards 
and allows developers to implement the interfaces using a broad range of computer 
languages. Using this framework, simulation models can be assembled by connecting 
component models in much the same way that Lego blocks are put together to assemble 
a house. ModCom thus allows modelers to create models and modeling tools that are 
easily exchanged (in binary form or source code) with colleagues across the hall 
or across the globe.

NMODCOM is a C# implementation based on the C++ code from Hillyer et al. (2003). The 
first version of NMODCOM was written in 2004 by Arjan Lamaker and Frits van Evert.

Link to [Documentation](doc/NModcom.md) .