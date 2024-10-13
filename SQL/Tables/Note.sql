CREATE TABLE Note (
  id SERIAL PRIMARY KEY,
  operator_id INTEGER REFERENCES Operator (id),
  company_id INTEGER,
  note_content TEXT,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);