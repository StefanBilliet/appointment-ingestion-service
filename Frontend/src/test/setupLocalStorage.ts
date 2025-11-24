type StorageHost = typeof globalThis & {
  localStorage?: Storage;
};

const globalScope = globalThis as StorageHost;

const ensureInScope = (scope: StorageHost | undefined, storage: Storage) => {
  if (!scope) {
    return;
  }
  if (scope.localStorage === storage) {
    return;
  }

  Object.defineProperty(scope, 'localStorage', {
    configurable: true,
    value: storage,
  });
};

export const ensureMemoryLocalStorage = () => {
  if (typeof globalScope.localStorage?.getItem === 'function') {
    return;
  }

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

  ensureInScope(globalScope, memoryStorage);
  ensureInScope((globalThis as StorageHost & { window?: StorageHost }).window, memoryStorage);
};

ensureMemoryLocalStorage();
