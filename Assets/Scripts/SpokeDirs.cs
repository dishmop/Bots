using UnityEngine;



public class SpokeDirs{
	public enum Dirs{
		kUp,
		kUpRght,
		kDownRight,
		kDown,
		kDownLeft,
		kUpLeft
	}
	
	static public Vector3 GetDirVector(SpokeDirs.Dirs dir){
		return GetDirVector((int)dir);
	}
	
	static public Vector3 GetDirVector(int dir){
		Vector3 up = new Vector3(0, 1, 0);
		Quaternion rot = Quaternion.Euler(0, 0, -60 * (float)dir);
		return rot * up;
	}
	
	static public SpokeDirs.Dirs CalcInverseSpoke(SpokeDirs.Dirs dir){
		return (SpokeDirs.Dirs)(((int) dir + 3) % 6);
	}
	static public int CalcInverseSpoke(int dir){
		return (dir + 3) % 6;
	}

}
