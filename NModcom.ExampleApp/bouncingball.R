g = 9.81

ballheight = function(v0, h0, t)
{
	h0 + v0 * t - 0.5 * g * t * t
}

t = seq(0,5, by=0.1)
plot(t, ballheight(0, 1, t), xlim=c(0,3), ylim=c(0,1))


euler=read.csv("bouncingball_euler.csv")
rk=read.csv("bouncingball_rk.csv")
points(euler$time, euler$height, col="red")
points(rk$time, rk$height, col="green")
