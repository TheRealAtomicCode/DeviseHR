CREATE TABLE Absence (
  id SERIAL PRIMARY KEY,
  employee_id INTEGER NOT NULL,
  company_id INTEGER NOT NULL,
  contract_id INTEGER NOT NULL,
  absence_start_date DATE NOT NULL,
  absence_end_date DATE NOT NULL,
  is_first_half_day BOOLEAN,
  is_days BOOLEAN NOT NULL DEFAULT true,
  start_time TIME NOT NULL,
  end_time TIME NOT NULL,
  days_deducted INTEGER,
  hours_deducted INTEGER,
  absence_type_id INTEGER NOT NULL CONSTRAINT fk_absence_type REFERENCES Absence_Type (id),
  comment VARCHAR(255),
  approved_by INT CONSTRAINT fk_approved_by_manager REFERENCES Employee (id),
  absence_state INT NOT NULL,
  approved_by_admin INT CONSTRAINT fk_approved_by_admin REFERENCES Employee (id),

  added_by INT NOT NULL,   
  updated_by INT,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

  -- CONSTRAINT check_approval CHECK ((is_pending = TRUE AND is_approved = FALSE) OR (is_pending = FALSE)),

  CONSTRAINT check_deduction_not_null CHECK (days_deducted IS NOT NULL OR hours_deducted IS NOT NULL),
  CONSTRAINT check_deducted_not_negative CHECK (days_deducted >= 0 OR hours_deducted >= 0),

  CONSTRAINT half_days_for_leaves_in_days CHECK (is_days = true),
  CONSTRAINT check_start_date_not_after_end_date CHECK (absence_start_date <= absence_end_date),

  CONSTRAINT fk_employee FOREIGN KEY (employee_id) REFERENCES Employee (id),
  CONSTRAINT fk_company FOREIGN KEY (company_id) REFERENCES Company (id),
  CONSTRAINT fk_contract FOREIGN KEY (contract_id) REFERENCES Contract (id),
  -- -1 unaproved
  -- 0 pending
  -- 1 approved by manager
  -- 2 approved by admin
  CONSTRAINT check_absence_state CHECK (absence_state IN (-1, 0, 1, 2))
);