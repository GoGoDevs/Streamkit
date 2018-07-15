using System;
using System.Drawing;

namespace Streamkit.Core {
    public abstract class Gadget {
        protected string id;
        protected User user;

        public Gadget(string id, User user) {

        }

        public abstract void Update();
    }


    public class Bitbar : Gadget {
        private int value = 0;
        private int maxValue = 0;
        private byte[] image = null;
        private Color color = Color.Blue;

        public Bitbar(string id, User user) : base(id, user) {

        }

        public override void Update() {
            throw new NotImplementedException();
        }

        public void AddBits(int count) {
            this.value += count;
        }
    }
}