import migrate from './migrate.ts';
import seed from './seed.ts';

await migrate();
await seed();
