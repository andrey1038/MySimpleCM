using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu
{
    static public class ThisProgram
    {
        public delegate void ExceptionHandler(Exception exception);
        static public event ExceptionHandler ProgramException;

        static public string Path_RemoveKeys
        {
            get
            {
#if DEBUG
                return @"C:\Users\Andrey\Desktop\RemoveKeys";
#else
                return AppDomain.CurrentDomain.BaseDirectory + "RemoveKeys";
#endif
            }
        }
        static public string Path_EmptyCommand
        {
            get
            {
#if DEBUG
                return @"C:\Users\Andrey\source\repos\C#\избранные проекты\EmptyCommand\EmptyCommand\bin\Release\EmptyCommand.exe";
#else
                return AppDomain.CurrentDomain.BaseDirectory + "EmptyCommand.exe";
#endif
            }
        }

        static public void IsExistsEnvironment()
        {
            if (!Directory.Exists(Path_RemoveKeys)) {
                Directory.CreateDirectory(Path_RemoveKeys);
            }

            if (!File.Exists(Path_EmptyCommand))
                ProgramException(new MyProgramException("Не обнаружен файл 'EmptyCommand.exe'.", TypeException.Fatal));
        }

        static public void ThrowException(Exception ex)
        {
            ProgramException(ex);
        }
    }


    public enum TypeException
    {
        Fatal = 0,
        NotFatal
    }
    public class MyProgramException : Exception
    {
        public MyProgramException(string message, TypeException type)
            : base(message)
        {
            this.Type = type;
        }

        public TypeException Type { get; private set; }
    }
}
