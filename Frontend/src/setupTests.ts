import {env} from 'node:process';

env.TZ = 'UTC';

import '@testing-library/jest-dom/vitest';
import './test/setupLocalStorage';
