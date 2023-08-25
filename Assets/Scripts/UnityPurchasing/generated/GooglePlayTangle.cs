// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("7aa/YElxz7dPnWp+P6vNMnZxKIPmRHqUxFVUfCpzEct/Aa8E7raL8Lc4x7mbhlmoNoJTRxrsUXDLEjRA2c5ns3x1rwwoDTT9nQ892mCFpgKaGRcYKJoZEhqaGRkY6jB5hIC+buehrBDodIOUIkkY0sjBQWJQb1qfxtdsSDYRG6ffcSNeJneYzYNfhmsomhk6KBUeETKeUJ7vFRkZGR0YG+3AstKHVmwSKMKLu7kA57vZO+bCx57MsQifXW2Lsl4dOVE3LPa8gItZeyc6kkQamkswCmKXn/0rfJ8iBAnwlcpGmHrNfAypP9fu1QEr0/1GA0aPm/TrxcoXY4X2SLQtEzt3u3PiO7JiXFNI17KoNv80wVndvrh6gcQSpZL8YX949RobGRgZ");
        private static int[] order = new int[] { 11,7,13,8,7,12,13,11,9,9,10,12,13,13,14 };
        private static int key = 24;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
