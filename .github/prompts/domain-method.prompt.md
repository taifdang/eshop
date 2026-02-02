---
agent: agent
model: Claude Sonnet 4.5 (copilot)
---
Define the task to achieve, including specific requirements, constraints, and success criteria.

# Task
Refactor this code following DDD.

# Requirements
1. Product is the Aggregate Root.
2. Child entities must NOT use DbContext or Repository.
3. All changes can be go through Product methods.
4. If the logic is complex, create a Service to implement the code (example: ProductService, VariantGenerator, productImageResolver, ...)
5. Add all dependences references for this class.

# Contraints
- No delete old code
- Only comment old code and add new code using: 
/* OLD [version] */
/* NEW [version] */
- If unsure, do not refactor. Ask for clarification.

### Output
- Code only
- No explanation
