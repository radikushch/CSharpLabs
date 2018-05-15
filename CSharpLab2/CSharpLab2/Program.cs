using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var Register = new Register();
            Register.Changed += ChangeListener;
            Register.Add(new RSTrigger("Trigger1", true, false));
            Register.Add(new JKTrigger("Trigger2", true, false));
            Register.Add(new RSTrigger("Trigger3", false, true));
            Register.Add(new JKTrigger("Trigger4", false, true));
            Register.Insert(2, new RSTrigger("Trigger0", false, false));
            Register.Add(new JKTrigger("Trigger5", false, false));
            Register.Add(new RSTrigger("Trigger5", true, false) & new RSTrigger("Triger6", false, true));
            Register.Delete(2);
            Console.WriteLine(Register);
            Register.ResetAllItems();
            Console.WriteLine(Register);

            try
            {
                Register.Get(48);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("IndexOutOfRangeException");
            }

            Console.ReadKey();
        }

        public static void ChangeListener(ChangeType change)
        {
            switch (change)
            {
                case ChangeType.Add:
                    Console.WriteLine("Element was added");
                    break;
                case ChangeType.Delete:
                    Console.WriteLine("Element was deleted");
                    break;
                case ChangeType.Set:
                    Console.WriteLine("Element was setted");
                    break;
                case ChangeType.Insert:
                    Console.WriteLine("Element was inserted");
                    break;
                case ChangeType.Reset:
                    Console.WriteLine("Elements were reseted");
                    break;
            }
        }
    }

    public interface ITrigger
    {
        void Reset();
    }

    public abstract class Trigger : ITrigger
    {
        protected string Identificator { get; set; }
        protected bool OutputValue { get; set; }

        public abstract override string ToString();
        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();
        public abstract void Reset();
    }

    public class RSTrigger : Trigger
    {
        private bool RInput;
        private bool SInput;

        public bool GetRInput()
        {
            return this.RInput;
        }
        public void SetRInput(bool value)
        {
            RInput = value;
            SetOutputValue();
        }

        public bool GetSInput()
        {
            return this.SInput;
        }
        public void SetSInput(bool value)
        {
            SInput = value;
            SetOutputValue();
        }

        private void SetOutputValue()
        {
            if (!RInput && SInput) OutputValue = true;
            if (RInput && !SInput) OutputValue = false;
            if (RInput && SInput) Console.WriteLine("Do not enter such combination");
        }
        public RSTrigger(string identificator, bool R, bool S)
        {
            this.Identificator = identificator;
            this.RInput = R;
            this.SInput = S;
        }

        public RSTrigger()
        {
            OutputValue = false;
        }

        public override void Reset()
        {
            SInput = false;
            RInput = false;
            SetOutputValue();
        }

        public RSTrigger DeepCopy()
        {
            RSTrigger result = new RSTrigger(this.Identificator, this.RInput, this.SInput);
            return result;
        }

        public override bool Equals(object obj)
        {
            RSTrigger o = (RSTrigger)obj;
            if (Identificator == o.Identificator && ((RInput && o.RInput) || (SInput && o.SInput))) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return RInput.GetHashCode() + 6214 * SInput.GetHashCode();
        }

        public override string ToString()
        {
            return " " + Identificator + " (RS-Trigger): { R = " + RInput + ", S = " + SInput + " -> " + OutputValue + " }";

        }

        public static RSTrigger operator &(RSTrigger x, RSTrigger y)
        {
            RSTrigger result = new RSTrigger
            {
                RInput = x.RInput & x.SInput
            };
            return result;
        }

        public static RSTrigger operator |(RSTrigger x, RSTrigger y)
        {
            RSTrigger result = new RSTrigger
            {
                RInput = x.RInput | x.SInput
            };
            return result;
        }

        public static bool operator ==(RSTrigger x, RSTrigger y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(RSTrigger x, RSTrigger y)
        {
            return !Equals(x, y);
        }

    }

    public class JKTrigger : Trigger
    {
        private bool JInput;
        private bool KInput;

        public bool GetRInput()
        {
            return this.JInput;
        }
        public void SetRInput(bool value)
        {
            JInput = value;
            SetOutputValue();
        }

        public bool GetSInput()
        {
            return this.KInput;
        }
        public void SetSInput(bool value)
        {
            KInput = value;
            SetOutputValue();
        }

        private void SetOutputValue()
        {
            if (!JInput && KInput) OutputValue = true;
            if (JInput && !KInput) OutputValue = false;
            if (JInput && KInput) OutputValue = !OutputValue;
        }
        public JKTrigger(string identificator, bool J, bool K)
        {
            this.Identificator = identificator;
            this.JInput = J;
            this.KInput = K;
        }

        public JKTrigger()
        {
            OutputValue = false;
        }


        public override void Reset()
        {
            KInput = false;
            JInput = false;
            SetOutputValue();
        }

        public JKTrigger DeepCopy()
        {
            JKTrigger result = new JKTrigger(this.Identificator, this.JInput, this.KInput);
            return result;
        }

        public override bool Equals(object obj)
        {
            JKTrigger o = (JKTrigger)obj;
            if (Identificator == o.Identificator && ((JInput && o.JInput) || (KInput && o.KInput))) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return JInput.GetHashCode() + 6214 * KInput.GetHashCode();
        }

        public override string ToString()
        {
            return " " + Identificator + " (JK-Trigger): { J = " + JInput + ", K = " + KInput + " -> " + OutputValue + " }";

        }

        public static JKTrigger operator &(JKTrigger x, JKTrigger y)
        {
            JKTrigger result = new JKTrigger
            {
                JInput = x.JInput & x.KInput
            };
            return result;
        }

        public static JKTrigger operator |(JKTrigger x, JKTrigger y)
        {
            JKTrigger result = new JKTrigger
            {
                JInput = x.JInput | x.KInput
            };
            return result;
        }

        public static bool operator ==(JKTrigger x, JKTrigger y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(JKTrigger x, JKTrigger y)
        {
            return !Equals(x, y);
        }


    }

    public enum ChangeType { Add, Insert, Delete, Set, Reset }
    public delegate void ChangeHandler(ChangeType change);

    public class Register
    {
        public List<Trigger> Elements
        {
            get;
        }

        public event ChangeHandler Changed;

        public Register()
        {
            Elements = new List<Trigger>();
        }

        public void Add(Trigger v)
        {
            Elements.Add(v);
            Changed?.Invoke(ChangeType.Add);
        }

        public Trigger Get(int index)
        {
            if (index < 0 || index >= Elements.Count) throw new IndexOutOfRangeException();
            return Elements[index];
        }

        public void Set(int index, Trigger v)
        {
            if (index < 0 || index >= Elements.Count) throw new IndexOutOfRangeException();
            Elements[index] = v;
            Changed?.Invoke(ChangeType.Set);
        }

        public void Insert(int index, Trigger v)
        {
            if (index < 0 || index >= Elements.Count) throw new IndexOutOfRangeException();
            Elements.Insert(index, v);
            Changed?.Invoke(ChangeType.Insert);
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= Elements.Count) throw new IndexOutOfRangeException();
            Elements.RemoveAt(index);
            Changed?.Invoke(ChangeType.Delete);
        }

        public void ResetAllItems()
        {
            Trigger obj;
            for (int i = 0; i < Elements.Count; i++)
            {
                obj = Get(i);
                obj.Reset();
            }
            Changed?.Invoke(ChangeType.Reset);
        }

        public override string ToString()
        {
            return "[" + string.Join("\n", Elements) + "]";
        }
    }

    public class MyExeption : Exception
    {
        MyExeption(String str) : base("From MyExeption: " + str) { }
    }
}
