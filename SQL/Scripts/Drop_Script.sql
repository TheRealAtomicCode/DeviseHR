
-- drop types
DROP TYPE IF EXISTS contract_record;

-- drop triggers


-- drop functions
-- DROP FUNCTION IF EXISTS edit_subordinates;

-- drop tables
-- DROP TABLE IF EXISTS leave_year;
-- DROP TABLE IF EXISTS Term CASCADE;
DROP TABLE IF EXISTS Absence CASCADE;
DROP TABLE IF EXISTS Working_Pattern CASCADE;
-- DROP TABLE IF EXISTS Discarded_Contract CASCADE;
DROP TABLE IF EXISTS Absence_Type CASCADE;

DROP TABLE IF EXISTS Hierarchy CASCADE;
DROP TABLE IF EXISTS Contract CASCADE;
DROP TYPE IF EXISTS Contract_Type_Enum CASCADE;


DROP TABLE IF EXISTS Employee;
DROP TABLE IF EXISTS Permission;
DROP TABLE IF EXISTS Company;
DROP TABLE IF EXISTS Note;
DROP TABLE IF EXISTS Operator;
DROP TYPE IF EXISTS operator_role_enum;
DROP TYPE IF EXISTS employee_permission_enum;
