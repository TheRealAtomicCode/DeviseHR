CREATE TABLE Hierarchy (
    id SERIAL PRIMARY KEY,
    manager_id INTEGER NOT NULL,
    subordinate_id INTEGER NOT NULL,
    CONSTRAINT fk_manager FOREIGN KEY (manager_id) REFERENCES Employee (id) ON DELETE CASCADE,
    CONSTRAINT fk_subordinate FOREIGN KEY (subordinate_id) REFERENCES Employee (id) ON DELETE CASCADE,
    CONSTRAINT uq_hierarchies UNIQUE (manager_id, subordinate_id),
    CHECK (manager_id <> subordinate_id)
);
