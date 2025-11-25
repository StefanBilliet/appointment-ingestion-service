/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_API_BASE_URL?: string;
  readonly API_HTTP?: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
