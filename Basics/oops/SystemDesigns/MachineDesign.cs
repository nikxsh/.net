using System;

namespace ObjectOriented.SystemDesigns
{
    class MachineDesign
    {
        private void Problem()
        {
			// Implement design
			// 1. Tank that drive & shoot
			// 2. Plane that fly
			// 3. Truck that drive

			Plane plane = new Plane();
			plane.Fly();
			plane.Shoot(new Tank());

			Tank tank = new Tank();
			tank.Drive();
			tank.Shoot(new Plane());

			Truck truck = new Truck();
			truck.Drive();
		}
    }

	public class GPS
	{
		public int Longitude { get; set; }
		public int Lattitude { get; set; }
	}

	public abstract class Machine
	{
		public GPS Location { get; set; }
		public double FuelCapacity { get; set; }
		public abstract void StartEngine();
	}

	public interface ICombat
	{
		void Shoot(Machine machine);
	}

	public class Plane : Machine, ICombat
	{
		public void Fly()
		{

		}

		public void Shoot(Machine machine)
		{
			throw new NotImplementedException();
		}

		public override void StartEngine()
		{
			throw new NotImplementedException();
		}
	}

	public class Tank : Machine, ICombat
	{
		public void Drive()
		{

		}

		public void Shoot(Machine machine)
		{
			throw new NotImplementedException();
		}

		public override void StartEngine()
		{
			throw new NotImplementedException();
		}
	}

	public class Truck : Machine
	{
		public void Drive()
		{

		}

		public override void StartEngine()
		{
			throw new NotImplementedException();
		}
	}
}
