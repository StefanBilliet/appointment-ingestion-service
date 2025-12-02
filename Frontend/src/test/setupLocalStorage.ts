type StorageHost = typeof globalThis & {
  localStorage?: Storage;
};

const globalScope = globalThis as StorageHost;

const installLocalStorage = (scope: StorageHost | undefined, storage: Storage) => {
  if (!scope) {
    return;
  }
  try {
    Object.defineProperty(scope, 'localStorage', {
      configurable: true,
      value: storage,
    });
  } catch {
    // Ignore if a host already has a non-configurable localStorage (e.g., browser)
  }
};

export const ensureMemoryLocalStorage = () => {
  const store = new Map<string, string>();
  const memoryStorage: Storage = {
    get length() {
      return store.size;
    },
    clear: () => {
      store.clear();
    },
    getItem: (key) => store.get(String(key)) ?? null,
    key: (index) => Array.from(store.keys())[index] ?? null,
    removeItem: (key) => {
      store.delete(String(key));
    },
    setItem: (key, value) => {
      store.set(String(key), String(value));
    },
  };

  installLocalStorage(globalScope, memoryStorage);
  installLocalStorage((globalThis as StorageHost & { window?: StorageHost }).window, memoryStorage);
};

ensureMemoryLocalStorage();
