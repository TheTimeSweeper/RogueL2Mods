namespace ResourceCountersMod {

    public class ResourceReporter {

        public event OnResourceChanged onPostResourceChanged;

        public delegate void OnResourceChanged(int amount);

        public void Invoke(int amount) {
            onPostResourceChanged?.Invoke(amount);
        }
    }
}
