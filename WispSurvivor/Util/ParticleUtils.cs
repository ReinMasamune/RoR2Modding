using System.Reflection;

namespace WispSurvivor.Helpers
{
    public static class ParticleUtils
    {
        public static void SetParticleStruct<T>( T str1, T str2 ) where T : struct
        {
            foreach( PropertyInfo p in typeof( T ).GetProperties() )
            {
                if( p.CanWrite && p.CanRead ) p.SetValue(str1, p.GetValue(str2));
            }
        }
    }
}
