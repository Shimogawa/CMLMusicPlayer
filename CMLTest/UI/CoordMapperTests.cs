using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMLMusicPlayer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLMusicPlayer.UI.Tests
{
	[TestClass()]
	public class CoordMapperTests
	{

		[TestMethod()]
		public void CoordMapperTest()
		{
			var mapper = new CoordMapper(10, 10);
			Assert.AreEqual(mapper.MaxX, 10);
			Assert.AreEqual(mapper.MaxY, 10);
			Assert.IsNotNull(mapper);

			mapper = new CoordMapper(999, 999);
			Assert.AreEqual(mapper.MaxX, 999);
			Assert.AreEqual(mapper.MaxY, 999);
			Assert.IsNotNull(mapper);
		}

		[TestMethod()]
		public void ResetTest()
		{
			var mapper = new CoordMapper(10, 10);
			var mapper1 = new CoordMapper(999, 999);
			mapper.Reset();
			mapper1.Reset();
		}

		[TestMethod()]
		public void SetTest()
		{
			var mapper = new CoordMapper(10, 10);
			mapper.Set(0, 0);

			Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
				mapper.Set(-1, -1);
			});

			Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
				mapper.Set(10, 10);
			});

		}

		[TestMethod()]
		public void ClearTest()
		{
			var mapper = new CoordMapper(10, 10);
			mapper.Set(0, 0);
			mapper.Clear(0, 0);
			var coord = mapper.QueryCoord(1, 1);
			Assert.AreEqual(coord, new Coordinate<int>(1, 1));

			Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
				mapper.Clear(-1, -1);
			});

			Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
				mapper.Clear(423423, 413847);
			});
		}

		[TestMethod()]
		public void QueryCoordTest()
		{
			var mapper = new CoordMapper(10, 10);
			var coord = mapper.QueryCoord(0, 0);
			Assert.AreEqual(coord, new Coordinate<int>(0, 0));

			coord = mapper.QueryCoord(5, 5);
			Assert.AreEqual(coord, new Coordinate<int>(5, 5));

			Assert.ThrowsException<ArgumentOutOfRangeException>( () => {
				coord = mapper.QueryCoord(10, 10);
			});

			Assert.ThrowsException<ArgumentOutOfRangeException>( () =>{
				coord = mapper.QueryCoord(-1, -1);
			});

			mapper.Set(4, 5);
			coord = mapper.QueryCoord(5, 5);
			Assert.AreEqual(coord, new Coordinate<int>(6, 5));

			mapper.Set(5, 5);
			coord = mapper.QueryCoord(6, 5);
			Assert.AreEqual(coord, new Coordinate<int>(8, 5));
		}

	}
}